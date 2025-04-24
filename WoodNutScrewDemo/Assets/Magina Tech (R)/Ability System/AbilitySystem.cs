using Hung.GameData.RPG;
using Hung.Pooling;
using Hung.Unit;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Hung.StatSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityExtensions;


namespace Hung.AbilitySystem
{
    public interface IAbility : IEffectable, IMono, ICooldown, IPosition, IDisposable
    {
        int rank { get; set; }

        int targetIndex { get; set; }

        int maxTarget { get; set; }

        int castTime { get; set; }

        IUnit unit { get; }

        List<IStatEffect> effects { get; }
    }

    public interface IAbilityHolder: IMono, IAbilityCastFlow
    {
        void Add(AbilityData ability);
    }

    public interface IAbilityCastFlow: IFlow
    {
        UnityEvent<int> castRelease { get; }

        void CastRelease(int castIndex);
    }
}


namespace Hung.AbilitySystem
{

    public delegate void RPG_Effect(IUnit unit);

    public enum MultiplierMethod
    {
        PrimarySpreading,
        SecondaryTarget
    }

    [Serializable]
    public struct SpawnOffset
    {
        public Vector2 position;
        [Range(0, 360)] public float angle;

        public bool followRotation;

        public Vector3 Get3DPostionOffset()
        {
            return new Vector3(position.x, 0, position.y);
        }
    }

    public interface ISkillPropertyOverride
    {
        bool isOverride { get; }
    }

    public interface ISpawnOffset: IStat
    {
        SpawnOffset spawnOffset { get; }
    }

    public interface IMultiTarget: IStat
    {
        bool multipleTarget { get; }

        MultiplierMethod multiplierMethod { get; } 

        int GetExtraTargetAmount(IAbility caster);
    }

    public interface IOutput<T>: IStat
    {      
        T Get(IAbility caster);

        T[] GetMulti(IAbility caster, int amount);
    }

    public interface IUnitTarget : IOutput<IUnit>, IMultiTarget
    {
    }

    public interface ILocationTarget : IOutput<Vector3>, IMultiTarget
    {

    }

    public interface IDirectionTarget : IOutput<Vector3>, IMultiTarget
    {

    }

    public interface ISkillshot : IMono, IPosition, IToggleable, IDisposable
    {
        VisualEffect3D vfx { get; set; }

        ISkillshot DOMoveLocation(Vector3 destination);

        ISkillshot DOMoveDirection(Vector3 direction);

        ISkillshot Redirect(float angle);

        ISkillshot DOFollow(IUnit target);

        ISkillshot OnUpdate(Action updateAction);

        ISkillshot OnComplete(Action completeAction);

        ISkillshot OnDispose(Action disposeAction);

        void Attach(IAbility unit, SpawnOffset offset);

        void InstantAffect(RPG_Effect effect);

        void DisappearAfter(float seconds);
    }

    public interface ISkillshotType
    {
        void OnCast(IAbility caster, ISkillTargetType targetType, ISkillshot skillshot);

        ISkillshot Spawn(IAbility caster, VisualEffect3D vfx, RPG_Effect effect = null);
    }

    public interface ISkillTargetType
    {
        bool InitTarget(IAbility caster);
    }

    public abstract class ExecuteTable
    {
        [field:SerializeField] public bool multipleCast { get; private set; }
        //[field:SerializeField] public int 
        [field: SerializeField] public bool redraw { get; protected set; }

        protected T GetTargetStat<T>(ISkillTargetType target) where T : IStat
        {
            return (T)target;
        }

        protected T GetSkillshotStat<T>(ISkillshot skillshot) where T : IStat
        {
            return (T)skillshot;
        }

        public virtual void PreExecute(IAbility caster, ref int spellForm)
        {
            spellForm = 0;
        }

        public abstract void Execute(IAbility caster, ISkillTargetType targetType, ISkillshot[] skillshots, RPG_Effect effect);

        public abstract bool CheckValid(ISkillTargetType targetType);

        public abstract bool CheckValid(ISkillshotType skillshotType);
    }
}


namespace Hung.AbilitySystem.Core
{
    public enum AOEShape : byte
    {
        Circle,
        Cone,
        Box
    }

    public class AOE_Core : ISkillshotType, ISpawnOffset
    {        
        public enum SpawnType: byte
        {
            Sending,
            Located
        }

        [SerializeField] private AOEShape shape;
        [ShowIf("coneShape")][Range(10, 240)][SerializeField] private float angle;
        [ShowIf("boxShape")][SerializeField] private Vector2 boxSize;
        [SerializeField] private SpawnType spawnType;
        [field:ShowIf("locatedSpawnType")][field: SerializeField] public SpawnOffset spawnOffset { get; private set; }

        bool circleShape => shape == AOEShape.Circle || shape == AOEShape.Cone;

        bool coneShape => shape == AOEShape.Cone;

        bool boxShape => shape == AOEShape.Box;

        bool locatedSpawnType => spawnType == SpawnType.Located;

        

        public ISkillshot Spawn(IAbility caster, VisualEffect3D vfx, RPG_Effect effect = null)
        {
            CommonPool.Spawn(out AreaOfEffect aoe);
            var spawnVFX = Pool.Spawn(vfx, aoe.transform);

            caster.TryToGetStat(out Area rad);
            aoe.Setup(caster, effect, spawnVFX, shape, angle, rad);

            if (caster is ISkillDuration dur)
            {

            }
            
            return aoe;
        }

        public void OnCast(IAbility caster, ISkillTargetType targetType, ISkillshot skillshot)
        {
            if (targetType is not IUnitTarget && targetType is not ILocationTarget)
            {
                if (locatedSpawnType)
                {
                    skillshot.Attach(caster, spawnOffset);
                }
            }
            else if (targetType is IUnitTarget targeter)
            {
                if (locatedSpawnType)
                {
                    skillshot.transform.position = targeter.Get(caster).positionValue;
                    if(caster.TryToGetStat(out Duration duration))
                    {
                        skillshot.DisappearAfter(duration.current);
                    }
                }
            }
        }
    }

    [Serializable]
    public struct ProjectileProperty
    {
        [SerializeField] public bool blockByUnit;
        //[ShowIf("blockByUnit")][SerializeField] public float blockSize;
        [ShowIf("blockByUnit")][SerializeField] public int blockCount;
    }

    public class Projectile_Core : ISkillshotType, ISpawnOffset
    {
        [HideLabel] [SerializeField] private ProjectileProperty property;

        [field: SerializeField] public SpawnOffset spawnOffset { get; private set; }

        public ISkillshot Spawn(IAbility caster, VisualEffect3D vfx, RPG_Effect effect = null)
        {
            CommonPool.Spawn(out Projectile projectile);
            
            var spawnVFX = Pool.Spawn(vfx, projectile.transform);

            caster.TryToGetStat(out Area area);
            caster.TryToGetStat(out Speed spd);

            projectile.Setup(property, effect, spawnVFX, caster.unit.teamID, area, spd);
            
            return projectile;
        }

        public void OnCast(IAbility caster, ISkillTargetType targetType, ISkillshot skillshot)
        {
            if (targetType is not IMultiTarget)
            {
                skillshot.Attach(caster, spawnOffset);
            }
            else
            {
                skillshot.transform.position = caster.positionValue;
            }
            

            if (targetType is IUnitTarget unit)
            {
                var target = unit.Get(caster);
                /*if (target != null) */skillshot.DOFollow(target);
            }
            else if (targetType is IDirectionTarget dir)
            {
                if (caster.TryToGetStat(out SkillRange range))
                {
                    skillshot.DOMoveLocation(skillshot.transform.position + dir.Get(caster) * range.current);
                }
                else
                {
                    skillshot.DOMoveDirection(dir.Get(caster));
                }
            }
        }
    }

    public class Summon_Core : ISkillshotType
    {
        public void OnCast(IAbility caster, ISkillTargetType targetType, ISkillshot skillshot)
        {

        }

        public ISkillshot Spawn(IAbility caster, VisualEffect3D vfx, RPG_Effect onHit = null)
        {
            return null;
        }
    }

    public class None_Core : ISkillshotType
    {
        public void OnCast(IAbility caster, ISkillTargetType targetType, ISkillshot skillshot)
        {
            //skillshot.Attach(caster.unit);
        }

        public ISkillshot Spawn(IAbility caster, VisualEffect3D vfx, RPG_Effect onHit = null)
        {
            CommonPool.Spawn(out Projectile temp);
            var spawnVFX = Pool.Spawn(vfx, temp.transform);
            return temp;
        }
    }
}

namespace Hung.AbilitySystem.Core
{
    public class Self_Core : ISkillTargetType
    {
        public bool InitTarget(IAbility caster)
        {
            return true;
        }
    }

    public class Direction_Core : ISkillTargetType, IDirectionTarget
    {
        public enum DirectionDecision
        {
            Facing,
            MostCrowded,
            CurrentTarget
        }

        [SerializeField] private DirectionDecision decision;

        [field: SerializeField] public bool multipleTarget { get; private set; }

        [field: ShowIf("multipleTarget")][field: SerializeField] public MultiplierMethod multiplierMethod { get; private set; }

        public int GetExtraTargetAmount(IAbility caster)
        {
            if (multipleTarget && caster.TryToGetStat(out Amount quantity))
            {
                return (int)quantity.current - 1;
            }
            else return 0;
        }

        public bool InitTarget(IAbility caster)
        {
            return true;
        }

        public Vector3 Get(IAbility caster)
        {
            switch (decision)
            {
                case DirectionDecision.Facing: return caster.unit.faceDirection;

                case DirectionDecision.CurrentTarget:
                    if (caster.unit.currentTarget != null)
                    {
                        return caster.unit.currentTarget.positionValue;
                    }
                    else return caster.unit.faceDirection;
            }
                return Vector3.forward;
            }

        public Vector3[] GetMulti(IAbility caster, int amount)
        {
            throw new NotImplementedException();
        }
    }

    public class Location_Core : ISkillTargetType, ILocationTarget
    {
        public bool multipleTarget => throw new NotImplementedException();

        public MultiplierMethod multiplierMethod => throw new NotImplementedException();

        public int GetExtraTargetAmount(IAbility caster)
        {
            throw new NotImplementedException();
        }

        public bool InitTarget(IAbility caster)
        {
            throw new NotImplementedException();
        }

        public Vector3 Get(IAbility caster)
        {
            throw new NotImplementedException();
        }

        public Vector3[] GetMulti(IAbility caster, int amount)
        {
            throw new NotImplementedException();
        }
    }

    public class Passive_Core : ISkillTargetType
    {
        public bool InitTarget(IAbility caster)
        {
            throw new NotImplementedException();
        }
    }
}