using Hung.AbilitySystem;
using Hung.Core.TextSystem;
using Hung.StatSystem;
using Hung.StatSystem.Binding;
using Hung.Unit;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Hung.GameData.RPG
{
    [CreateAssetMenu(menuName = "Hung/Data/Ability")]
    public class AbilityData : GameData, IStatHolder<AbilityBindingStat>
    {
        [Serializable]
        public struct SkillEffectScaler
        {
            [SerializeReference] public IStatEffect effect;
            //[NonSerialized, OdinSerialize] public UnitBindingStat bindingStat;
        } 

        public struct SkillRanking
        {
            [LabelText("Ability Stat")][field: NonSerialized, OdinSerialize] public AbilityBindingStat StatHolder;

            [NonSerialized, OdinSerialize] public SkillEffectScaler[] SkillEffect;
            [field: SerializeField] public bool FunctionModify { get; private set; }
            [field: ShowIf("FunctionModify"), SerializeReference] public ExecuteTable ExecuteTable { get; private set; }
            [field: ShowIf("FunctionModify"), SerializeReference] public ISkillTargetType TargetType { get; private set; }
            [field: ShowIf("FunctionModify"), SerializeReference] public ISkillshotType Skillshot { get; private set; }
        }

        ExecuteTable[][] _executeAtRank;
        ISkillTargetType[] _targetTypeAtRank;
        ISkillshotType[] _skillshotTypeAtRank;

        [field: SerializeField] public VisualEffect3D Visual { get; private set; }

        [field: SerializeReference] public ExecuteTable ExecuteTable { get; private set; }

        [field: SerializeReference] public ISkillTargetType TargetType { get; private set; }
        [field: SerializeReference] public ISkillshotType Skillshot { get; private set; }

        [field: LabelText("Ability Stat")][field: NonSerialized, OdinSerialize] public AbilityBindingStat StatHolder { get; private set; }

        [field: NonSerialized, OdinSerialize] public SkillEffectScaler[] SkillEffect { get; private set; }

        [field: NonSerialized, OdinSerialize, ListDrawerSettings(ShowIndexLabels = true)] public SkillRanking[] Ranking { get; private set; }


        public bool needRedraw => ExecuteTable != null ? ExecuteTable.redraw : true;
      
        public override void Init()
        {
            //Debug.Log(name + " is init");
            var rankCount = Ranking.Length + 1;
            _executeAtRank = new ExecuteTable[rankCount][];
            _executeAtRank[0] = new ExecuteTable[] { ExecuteTable };
            for (int i = 0; i < rankCount - 1; i++)
            {
                var cur = Ranking[i].ExecuteTable;
                var lastArr = _executeAtRank[i];
                if (!Ranking[i].FunctionModify || cur == null)
                    _executeAtRank[i + 1] = lastArr;
                else if (cur is ISkillPropertyOverride executeOverride && executeOverride.isOverride)
                {
                    _executeAtRank[i + 1] = new ExecuteTable[] { cur };
                }
                else
                {
                    _executeAtRank[i + 1] = new ExecuteTable[lastArr.Length + 1];
                    lastArr.CopyTo(_executeAtRank[i + 1], 0);
                    _executeAtRank[i + 1][lastArr.Length] = cur;
                }
            }


            _targetTypeAtRank = new ISkillTargetType[rankCount];
            _targetTypeAtRank[0] = TargetType;
            for (int i = 1; i < rankCount; i++)
            {
                var cur = Ranking[i - 1].TargetType;
                if (Ranking[i - 1].FunctionModify && cur != null)
                {
                    _targetTypeAtRank[i] = cur;
                }
                else
                {
                    _targetTypeAtRank[i] = _targetTypeAtRank[i - 1];
                }
            }

            _skillshotTypeAtRank = new ISkillshotType[rankCount];
            _skillshotTypeAtRank[0] = Skillshot;
            for (int i = 1; i < rankCount; i++)
            {
                var cur = Ranking[i - 1].Skillshot;
                if (Ranking[i - 1].FunctionModify && cur != null)
                {
                    _skillshotTypeAtRank[i] = cur;
                }
                else
                {
                    _skillshotTypeAtRank[i] = _skillshotTypeAtRank[i - 1];
                }
            }
        }

        public IStatEffect SerializeEffect()
        {
            return SkillEffect[0].effect.Serialize();
        }

        public int Precast(IAbility caster)
        {
            int res = 0;
            var rank = caster.rank;
            var ExecuteTables = _executeAtRank[rank];
            if (ExecuteTables != null)
            {
                for (int i = 0; i < ExecuteTables.Length; i++)
                {
                    ExecuteTables[i]?.PreExecute(caster, ref res);
                }
                return res;
            }
            else
            {
                return 0;
            }
        }

        public void Cast(IAbility caster)
        {
            var rank = caster.rank;
            var TargetType = _targetTypeAtRank[rank];
            var Skillshot = _skillshotTypeAtRank[rank];
            var ExecuteTables = _executeAtRank[rank];

            void ApplyEffect(IUnit target)
            {
                ApplyEffectAndSpawnText(caster, target);
            }

            if (!TargetType.InitTarget(caster)) return;

            ISkillshot[] skillshots;

            if (TargetType is IMultiTarget multi)
            {
                int extra = multi.GetExtraTargetAmount(caster);
                //Debug.Log(extra); 
                skillshots = new ISkillshot[extra + 1];
                for(int i = 0; i < extra + 1; i++)
                {
                    var skillshot = Skillshot.Spawn(caster, Visual, ApplyEffect);
                    Skillshot.OnCast(caster, TargetType, skillshot);
                    skillshots[i] = skillshot;
                }
            }
            else
            {
                int amount = 1;
                if (caster.TryToGetStat(out Amount quantity))
                {
                    amount = (int)quantity.current;
                }
                skillshots = new ISkillshot[amount]; 
                for (int i = 0; i < amount; i++)
                {
                    var skillshot = Skillshot.Spawn(caster, Visual, ApplyEffect);
                    Skillshot.OnCast(caster, TargetType, skillshot);
                    skillshots[i] = skillshot;
                }
            }
            caster.castTime++;
            if (ExecuteTables != null)
            {
                for (int i = 0; i < ExecuteTables.Length; i++)
                {
                    ExecuteTables[i]?.Execute(caster, TargetType, skillshots, ApplyEffect);
                }
            }     
        }

        void ApplyEffectAndSpawnText(IAbility caster, IUnit target)
        {
            //Debug.Log("effect pre-value: " + caster.effects[0].value); 
            var effectValue = caster.effects[0].Apply(target);
            //Debug.Log("effect on " + target.gameObject.name + ": " + effectValue); 
            TextSpawner.Instance.Spawn(target.positionValue, effectValue);
        }

        public void RankUp(IAbility caster, int levelIndex)
        {
            caster.rank = levelIndex;
            var rankInfo = Ranking[levelIndex - 1];
            var rankingEffects = rankInfo.SkillEffect;
            if (rankingEffects != null && rankingEffects.Length > 0) caster.effects[0].Merge(rankingEffects[0].effect);

            foreach (var bindingStat in rankInfo.StatHolder.GetAll())
            {
                //Debug.Log(bindingStat.Key + ": " + bindingStat.Value);
                switch (bindingStat.Key)
                {
                    case AbilityStat.Cooldown:
                        if (caster.TryToGetStat(out Cooldown cd))
                        {
                            cd.SetValue(cd.current + bindingStat.Value);
                        }
                        break;

                    case AbilityStat.Duration:
                        if (caster.TryToGetStat(out Duration dur))
                        {
                            dur.SetValue(dur.current + bindingStat.Value);
                        }
                        break;

                    case AbilityStat.Area:
                        if (caster.TryToGetStat(out Area area))
                        {
                            area.SetValue(area.current + bindingStat.Value);
                        }
                        break;

                    case AbilityStat.Speed:
                        if (caster.TryToGetStat(out Speed spd))
                        {
                            spd.SetValue(spd.current + bindingStat.Value);
                        }
                        break;

                    case AbilityStat.Quantity:
                        if (caster.TryToGetStat(out Amount quantity))
                        {
                            quantity.SetValue(quantity.current + bindingStat.Value);
                        }
                        break;

                    case AbilityStat.Range:
                        if (caster.TryToGetStat(out SkillRange range))
                        {
                            range.SetValue(range.current + bindingStat.Value);
                        }
                        break;                    
                }
            }
        }
    }
}


