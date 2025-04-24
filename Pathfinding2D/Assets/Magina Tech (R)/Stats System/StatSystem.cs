using Hung.AbilitySystem;
using Hung.StatSystem;
using Hung.Unit;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using static Hung.StatSystem.Durability;

namespace Hung.StatSystem
{
    public static class StatSystem
    {
        public static int ID_COUNTER;

        public const float TICK_TIME = 0.25f;

        public const float TICK_TIME_DOUBLE = 0.5f;

        public static readonly WaitForSeconds DOT_DELAY = new WaitForSeconds(StatSystem.TICK_TIME);

        public const float CONSTANT_DURATION = 999f;


        public static bool TryToGetStat<T>(this IStat target, out T stat) where T : Stat
        {
            if (target is IStat<T> temp && temp.GetStat().floor >= 0)
            {
                stat = temp.GetStat();
                return true;
            }
            else
            {
                stat = default(T);
                return false;
            }
        }

        public static bool TryToGetStat<T>(this IUnit unit, out T stat) where T : Stat
        {
            if (unit is IUnitStat<T> temp && temp.GetStat().floor >= 0)
            {
                stat = temp.GetStat();
                return true;
            }
            else
            {
                stat = default(T);
                return false;
            }
        }

        public static bool TryToGetStat<T>(this IAbility caster, out T stat) where T : Stat
        {
            if (caster is IAbilityStat<T> temp && temp.GetStat().floor >= 0)
            {
                stat = temp.GetStat();
                return true;
            }
            else
            {
                stat = default(T);
                return false;
            }
        }

        public static string GetStatInfo<T>(string prefix, IStat target) where T : Stat
        {
            if (target is IStat<T> statHolder)
            {
                var stat = statHolder.GetStat();
                return $"{prefix}: {stat.current} / {stat.max}";
            }
            else
            {
                return prefix;
            }
        }

        public static string GetShortName(this Stat stat)
        {
            if (stat == null) throw new ArgumentNullException(nameof(stat));

            Type statType = stat.GetType();

            return statType.Name switch
            {
                "MovementSpeed" => "Speed",
                "Cooldown" => "CD",
                "AttackDamage" => "AD",
                "HealthPoint" => "HP",
                "Armor" => "Armor",
                "Evasion" => "EVA",
                _ => statType.Name 
            };
        }
    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.Durability>(\"Durability\", this)")]
    public class Durability : Stat, IUnitStat
    {
        public Durability(float value) : base(value)
        {

        }
    }
    [Serializable]
    public class Fury: Stat, IUnitStat
    {
        public Fury(float value): base(value)
        {

        }
    }

    [Serializable]
    public class Energy: Stat, IUnitStat
    {
        public Energy(float value): base(value)
        {

        }
    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.Price>(\"Price\", this)")]
    public class Price: Stat, IUnitStat
    {
        public Price(float value): base(value)
        {

        }
    }

    [Serializable/*, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.Level>(\"LV\", this)")*/]
    public class Level: Stat, IUnitStat, IAbilityStat
    {
        public const int DEFAULT_LEVEL = 0;

        public Level(float value): base(value)
        {

        }

        public Level(): base(DEFAULT_LEVEL)
        {

        }
    }

    [Serializable]
    public class Critical : Stat
    {

        public const float DEFAULT_CRITICAL_DAMAGE = 1.4f;

        public Critical(float value) : base(value)
        {
        }

        public override void Refresh()
        {
            floor = DEFAULT_CRITICAL_DAMAGE;
            base.Refresh();
        }
    }

    [Serializable]
    public class AreaEffect : Stat
    {
        public AreaEffect(float value) : base(value)
        {
        }

        public override void Refresh()
        {
            //floor = 0;
            base.Refresh();
        }
    }

    [Serializable]
    public class Area : Stat
    {
        public static float DEFAULT_VALUE = 1;

        public Area() : base(DEFAULT_VALUE) { }

        public Area(float value) : base(value)
        {
        }
    }

    [Serializable]

    public class Speed : Stat
    {
        public static float DEFAULT_VALUE = 1;

        public Speed() : base(DEFAULT_VALUE) { }

        public Speed(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class TimerSpeed : Speed
    {
        public TimerSpeed(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class AttackDamage : Stat, IUnitStat
    {

        public AttackDamage(float value) : base(value)
        {
        }

        public AttackDamage(): base(10)
        {

        }
    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.HealthPoint>(\"HP\", this)")] 
    public class HealthPoint : Stat, IUnitStat
    {
        public HealthPoint(float value) : base(value)
        {
        }

        public HealthPoint() : base(500)
        {

        }
    }

    [Serializable]
    public class Evasion : Stat, IUnitStat
    {
        public Evasion(float value) : base(value)
        {
        }
    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.MovementSpeed>(\"MS\", this)")]
    public class MovementSpeed : Speed, IUnitStat
    {
        public MovementSpeed(float value) : base(value)
        {
        }

        public static MovementSpeed operator +(MovementSpeed ms, StatChangerSource<MovementSpeed> statChanger)
        {
            ms.Change(statChanger.GetDirectValue(ms));
            return ms;
        }

    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.Cooldown>(\"CD\", this)")]
    public class Cooldown : Stat, IUnitStat, IAbilityStat
    {
        public Cooldown(float value) : base(value)
        {

        }
        public Cooldown(): base(0)
        {

        }
    }

    [Serializable]
    public class Duration : Stat
    {
        public Duration(float value) : base(value)
        {
        }
    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.Quantity>(\"QUA\", this)")]
    public class Quantity: Stat, IUnitStat, IAbilityStat
    {
        public static readonly int DEFAULT_VALUE = 1;

        public Quantity(): base(DEFAULT_VALUE)
        {

        }

        public Quantity(float value): base(value)
        {

        }
    }

    [Serializable, LabelText("@Hung.StatSystem.StatSystem.GetStatInfo<Hung.StatSystem.Amount>(\"Amount\", this)")]
    public class Amount : Stat, IUnitStat, IAbilityStat
    {
        public static readonly int DEFAULT_VALUE = 1;

        public Amount() : base(DEFAULT_VALUE) { }

        public Amount(float value) : base(value)
        {
        }
    }

    [Serializable]
    public class SkillRange : Stat
    {
        public static readonly float DEFAULT_VALUE = 3;

        public SkillRange(float value) : base(value)
        {

        }

        public SkillRange() : base(DEFAULT_VALUE)
        {

        }
    }

    [Serializable]
    public class Armor : Stat, IUnitStat
    {
        public static readonly float DEFAULT_VALUE = 0;

        public float damageReduction => current / (current + 100);
     
        public Armor(float value): base(value)
        {

        }

        public Armor(): base(DEFAULT_VALUE)
        {

        }
    }

    [Serializable]
    public class Stat
    {
        [field: SerializeField, InlineButton("Refresh"), InlineProperty] public EnormousValue floor { get; protected set; }
        [field: SerializeField, HorizontalGroup(LabelWidth = 50)][field: ReadOnly, InlineProperty] public EnormousValue current { get; protected set; }
        [field: SerializeField, HorizontalGroup(LabelWidth = 30)][field: ReadOnly, InlineProperty] public EnormousValue max { get; protected set; }
        [field: SerializeField] public bool overvalue { get; set; }

        [field: ReadOnly, SerializeField] public Modifier modifier { get; protected set; }

        public event Action<float> OnValueChange;

        public event Action<Modifier> OnModifierChange;

        public bool isFull => current >= max;

        public bool isEmpty => current <= 0;
        
        public float percent => (float)(current / max);

        public EnormousValue nextFloor { get; protected set; }
        public EnormousValue nextMax { get; protected set; }
        public EnormousValue nextCurrent { get; protected set; }


        public Stat(float value)
        {
            SetValue(value);
        }

        public void SetFloor(float value)
        {
            floor = value;
            UpdateValues();
        }

        public void SetNextFloor(float value)
        {
            var currentMax = max;
            nextFloor = value;
            nextCurrent = current;

            nextMax = Mathf.Max(0, (nextFloor + modifier.bonus) * modifier.percent * modifier.multiplier - modifier.minus);
            nextCurrent = Mathf.Clamp(nextCurrent + nextMax - currentMax, 0, max);

        }

        public void SetValue(float value)
        {
            floor = value;
            Refresh();
        }

        public void SetCurrent(float value)
        {
            current = Mathf.Clamp(value, 0, max);
            OnValueChange?.Invoke(current);
        }

        public void Empty()
        {
            current = 0;
            OnValueChange?.Invoke(current);
        }

        public void Fill()
        {
            current = max;
            OnValueChange?.Invoke(current);
        }

        public float GetScaled(float scale)
        {
            return current * scale;
        }

        public void ClearAllListeners()
        {
            OnValueChange = null;
        }

        public virtual void Refresh()
        {
            current = floor;
            max = floor;
            modifier = new Modifier();
            OnValueChange?.Invoke(current);
            OnModifierChange?.Invoke(modifier);
        }

        public float Change(float signedValue)
        {
            float _pre = current;
            //if (this is Quantity) Debug.Log(GetType().Name + ": " + current + "/" + max + " -> " + signedValue);
            current = Mathf.Clamp(current + signedValue, 0, !overvalue ? max : Mathf.Infinity);
            
            if (current != _pre)
            {
                //Debug.Log("Kekw"); 
                OnValueChange?.Invoke(current);                
            }
            return current - _pre;
        }

        public float AttempChange(float signedValue)
        {
            float _attemp = Mathf.Clamp(current + signedValue, 0, max);
            if (_attemp == 0) return signedValue;
            return _attemp - current;
        }

        public static Stat operator &(Stat stat, Modifier modifier)
        {
            var pre = stat.modifier.Copy();
            stat.modifier &= modifier;
            stat.UpdateValues();
            
            stat.OnModifierChange?.Invoke(stat.modifier - pre);
            return stat;
        }

        public static Stat operator +(Stat stat, float flat)
        {
            var pre = stat.modifier.Copy();
            stat.modifier += flat;
            stat.UpdateValues();
            stat.OnModifierChange?.Invoke(stat.modifier - pre);
            return stat;
        }

        public void MaxPlus(float flat)
        {
            modifier += flat;
            UpdateValues(alsoCurrent: false);
        }

        public static Stat operator -(Stat stat, float flat)
        {
            var pre = stat.modifier.Copy();
            stat.modifier -= flat;
            stat.UpdateValues();
            stat.OnModifierChange?.Invoke(stat.modifier - pre);
            return stat;
        }

        public static Stat operator ^(Stat stat, float percent)
        {
            var pre = stat.modifier.Copy();
            stat.modifier ^= percent;
            stat.UpdateValues();
            stat.OnModifierChange?.Invoke(stat.modifier - pre);
            return stat;
        }

        public static Stat operator *(Stat stat, float multiplier)
        {
            var pre = stat.modifier.Copy();
            stat.modifier *= multiplier;
            stat.UpdateValues();
            stat.OnModifierChange?.Invoke(stat.modifier - pre);
            return stat;
        }

        public static Stat operator /(Stat stat, float divider)
        {
            var pre = stat.modifier.Copy();
            stat.modifier /= divider;
            stat.UpdateValues();
            stat.OnModifierChange?.Invoke(stat.modifier - pre);
            return stat;
        }

        void UpdateValues(bool alsoCurrent = true)
        {
            var premax = max;
            max = Mathf.Max(0, (floor + modifier.bonus) * modifier.percent * modifier.multiplier - modifier.minus);
            if (alsoCurrent)
            {
                current = Mathf.Clamp(current + max - premax, 0, max);
                OnValueChange?.Invoke(current);
            }
        }

        [Serializable]
        public record Modifier
        {
            [field:SerializeField] public float bonus { get; private set; }
            [field:SerializeField] public float minus { get; private set; }
            [field:SerializeField] public float percent { get; private set; }

            [field:SerializeField] public float multiplier { get; set; }

            public Modifier()
            {
                bonus = 0;
                minus = 0;
                percent = 1;
                multiplier = 1;
            }

            public Modifier(Modifier modifier)
            {
                this.bonus = modifier.bonus;
                this.minus = modifier.minus;
                this.percent = modifier.percent;
                this.multiplier = modifier.multiplier;
            }

            public Modifier(float bonus, float minus, float percent, float multiplier)
            {
                this.bonus = bonus;
                this.minus = minus;
                this.percent = percent;
                this.multiplier = multiplier;
            }

            public Modifier Copy()
            {
                return new Modifier(this);
            }

            public static Modifier operator -(Modifier a, Modifier b)
            {
                return new Modifier(a.bonus - b.bonus, a.minus - b.minus, a.percent - b.percent + 1, a.multiplier / b.multiplier);
            }

            public static Modifier operator &(Modifier a, Modifier b)
            {
                //Debug.Log($"Modifier merge: {a.multiplier} & {b.multiplier}"); 
                return new Modifier(a.bonus + b.bonus, a.minus + b.minus, a.percent + b.percent - 1, a.multiplier * b.multiplier);
            }

            public static Modifier operator +(Modifier modifier, float flat)
            {
                var res = modifier.Copy();
                if (flat >= 0)
                {
                    res.bonus += flat;
                }
                else
                {
                    res.minus -= flat;
                }
                return res;
            }

            public static Modifier operator -(Modifier modifier, float flat)
            {
                var res = modifier.Copy();
                if (flat >= 0)
                {
                    res.bonus -= flat;
                }
                else
                {
                    res.minus += flat;
                }
                return res;
            }

            public static Modifier operator ^(Modifier modifier, float percent)
            {
                var res = modifier.Copy();
                res.percent += percent;
                return res;
            }

            public static Modifier operator *(Modifier modifier, float multiplier)
            {
                var res = modifier.Copy();
                res.multiplier *= multiplier;
                return res;
            }

            public static Modifier operator /(Modifier modifier, float divider)
            {
                var res = modifier.Copy();
                res.multiplier /= divider;
                return res;
            }
        }
    }

    [Serializable]
    public record Stacking
    {
        public int stack;
        public float refreshTime;

        public Stacking(float refreshTime)
        {
            this.stack = 0;
            this.refreshTime = refreshTime;
        }

        public Stacking(int stack, float refreshTime)
        {
            this.stack = stack;
            this.refreshTime = refreshTime;
        }
    }
   
}

public interface IPushable: IEffectable
{

}

public interface IHealthScaleable: IEffectable, IHP, IModifyStat<HealthUp, HealthPoint>
{

}

public interface IMoveable: IEffectable, IMovementSpeed, IModifyStat<Slow, MovementSpeed>, IModifyStat<Haste, MovementSpeed>
{

}

public interface IDamageable: IEffectable, IHP, IChangeStat<Damage, HealthPoint>
{
    Transform Head { get; }
}

public interface IRegenation: IEffectable, IHP, IHPRegen, IChangeStat<Healing, HealthPoint>
{
    void Regenerate();
}

public interface IHealable: IEffectable, IHP, IChangeStat<Healing, HealthPoint>, IModifyStat<Healing, HealthPoint>
{
     
}

public interface IAttackable: IEffectable, IATK, IATKSpeed, IModifyStat<Windfury, TimerSpeed>, IModifyStat<Cripple, TimerSpeed>, IModifyStat<ATKInscrease, AttackDamage>
{

}

public interface IHaveCooldown: ICooldown, IModifyStat<CooldownReduction, Stat>
{

}

public interface IEffectable: IStat
{
    
}

public interface IThreshold<Stat>
{
    Action OnMin { get; set; }
    Action OnMax { get; set; }
}

public interface IModifyStat<T, S> where T: StatChangerSource<S> where S : Stat
{
    void TakeModify(T statChanger, Action onModifyEnd = null);

    void RemoveModify(T statChanger);
}

public struct DamageDealtInfo
{
    public float actual;
    public float expected;
}

public interface IChangeStat<T, S> where T: StatChangerSource<S> where S : Stat
{
    DamageDealtInfo TakeChange(T statChanger);
}

public interface IDurability : IUnitStat<Durability>
{
    Durability Durability { get; }
}

public interface IFury: IUnitStat<Fury>, IThreshold<Fury>
{
    Fury Fury { get; }
}

public interface IEnergy: IUnitStat<Energy>, IThreshold<Energy>
{
    Energy Energy { get; }
}

public interface IQuantity : IStat<Quantity>
{
    Quantity Quantity { get; }
}

public interface IAmount : IStat<Amount>
{
    Amount Amount { get; }
}

public interface IPrice: IStat<Price>
{
    Price Price { get; }
}

public interface ILevelable
{
    Level Level { get; }
}

public interface ILevel : IStat<Level>
{
    Level LV { get; }

    Stat LVProcess { get; }
}

public interface IAOE: IUnitStat<AreaEffect>
{
    AreaEffect AOE{ get; }
}

public interface IHP: IUnitStat<HealthPoint>, IThreshold<HealthPoint>
{
    HealthPoint HP { get; }
}

public interface IHPRegen: IUnitStat<Stat>
{
    Stat HPRegen { get; }
}

public interface IATK: IUnitStat<AttackDamage>
{
    AttackDamage ATK { get; }
}

public interface ICritical: IUnitStat<Critical>
{
    Critical CriticalDMG { get; }
}

public interface IArmor: IUnitStat<Armor>
{
    Armor Armor { get; }
}

public interface IEvasion: IUnitStat<Evasion>
{
    Evasion EVA { get; }
}

public interface IATKRange: IUnitStat<SkillRange>
{
    SkillRange ATKRange { get; }
}

public interface IATKSpeed: IUnitStat<TimerSpeed>
{
    TimerSpeed ATKSpeed { get; }
}

public interface IMovementSpeed: IUnitStat<MovementSpeed>
{
    MovementSpeed MS { get; }
}

public interface ICooldown : IAbilityStat<Cooldown>
{
    Cooldown CD { get; }


}

public interface ISkillDuration: IAbilityStat<Duration>
{
    Duration DUR { get; }
}

public interface ISkillSpeed: IAbilityStat<Speed>
{
    Speed SPD { get; }
}

public interface ISkillArea: IAbilityStat<Area>
{
    Area AREA { get; }
}

public interface ISkillCone: ISkillArea
{
    float ConeAngle { get; }
}


public interface ISkillRange: IAbilityStat<SkillRange>
{
    SkillRange RNG { get; } 
}

public interface IUnitStat<T>: IStat<T> where T: Stat
{
    
}

public interface IAbilityStat<T> : IStat<T> where T : Stat
{

}

public interface IStat<T>: IStat where T : Stat
{
    float[] rankingValues { get; set; }

    T GetStat();

    StatChanging OnStatEffected { get; set; }
}

public delegate void StatChanging(float current, float max);

public interface IUnitStat: IStat
{

}

public interface IAbilityStat: IStat
{

}


public interface IStat
{

}
