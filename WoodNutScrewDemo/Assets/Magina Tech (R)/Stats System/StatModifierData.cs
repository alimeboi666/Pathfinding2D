using System;
using System.Collections.Generic;
using Hung.GameData;
using Hung.GameData.RPG;
using Hung.StatSystem.Binding;
using Hung.Unit;
using UnityEngine;
using static Hung.StatSystem.Stat;

namespace Hung.StatSystem
{
    public interface IModifiedEffectable: IEffectable
    {
        Dictionary<int, IStatEffect> effects { get; }
    }

    public interface IStatModify<T>: IEffectable where T : Stat
    {
        Modifier Modifier { get; set; }

        event Action<Modifier> OnModifierChanged;
    }

    public class ModifyBindingStat : BindingStat<UnitStat, Modifier>
    {
        protected override void ConvertStat(IStat target, UnitStat statType, Modifier value)
        {
            switch (statType)
            {
                case UnitStat.MS:
                    if (target is IStatModify<MovementSpeed> ms)
                    {
                        ms.Modifier &= value;
                    };
                    break;

                case UnitStat.Amount:
                    if (target is IStatModify<Amount> amount)
                    {

                    amount.Modifier &= value; 
                    } 
                    break;
            }
            
        }
    }

    [CreateAssetMenu(menuName = "Hung/Data/Modifier Source")]
    public class StatModifierData: GameData.GameData
    {
        [field:SerializeField] public ModifyBindingStat BindingModifies { get; private set; }
        public IUnit source { get; set; }
        public float value { get; set; }

        public float Apply(IEffectable statModifer)
        {
            BindingModifies.BuildStats(statModifer);
            if (statModifer is IModifiedEffectable modified)
            {
                //modified.effects
            }
            return 0;
        }

        public bool Merge(IStatEffect bonusEffect)
        {
            throw new NotImplementedException();
        }

        public IStatEffect Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
