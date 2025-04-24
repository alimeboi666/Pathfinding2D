using Hung.GameData.RPG;
using Hung.StatSystem;
using Hung.Unit;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]

public record Pacify: StatChangerSource<Fury>
{
    public Pacify(): base(0, 0 , Calculator.MaxPercent)
    {
        sign = -1;
    }
}

[Serializable]
public record Enrage: StatChangerSource<Fury>
{
    public Enrage() : base()
    {
        sign = +1;
    }

    public Enrage(float value): base(value, 0, Calculator.Direct)
    {
        sign = +1;
    }
}

[Serializable]
public record EnergyLost: StatChangerSource<Energy>
{
    public EnergyLost(): base()
    {
        sign = -1;
    }

    public EnergyLost(float value): base(value, 0, Calculator.Direct)
    {
        sign = -1;
    }
}

[Serializable]
public record EnergyCharging: StatChangerSource<Energy>
{
    public EnergyCharging(): base()
    {
        sign = +1;
    }

    public EnergyCharging(float value): base(value, 0, Calculator.Direct)
    {
        sign = +1;
    }
}

[Serializable]
public record RampUp: StatChangerSource<Quantity>
{
    public RampUp() : base()
    {
        sign += 1;
    }

    public RampUp(float value, Calculator calculator = Calculator.Direct) : base(value, 0, calculator)
    {
        sign = +1;
    }
}

[Serializable]
public record AmountUp: StatChangerSource<Amount>
{
    public AmountUp(): base()
    {
        sign += 1;
    }

    public AmountUp(float value, Calculator calculator = Calculator.Direct) : base(value, 0, calculator)
    {
        sign = +1;
    }
}

[Serializable]
public record Production: StatChangerSource<Amount>
{
    public Production(): base()
    {
        sign = +1;
    }

    public Production(float value, Calculator calculator = Calculator.MaxPercent): base(value, 0, calculator)
    {
        sign = +1;
    }
}

[Serializable]
public record PriceUp: StatChangerSource<Price>
{
    public PriceUp(): base()
    {
        sign = +1;
    }

    public PriceUp(float value = 0, Calculator calculator = Calculator.FloorPercent): base (value, 0, calculator)
    {
        sign = +1;
    }
}

[Serializable]
public record BonusCriticalDamage: StatChangerSource<Critical>
{
    public BonusCriticalDamage(): base()
    {
        sign = +1;
    }

    public BonusCriticalDamage(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime)
    {
        sign = +1;
    }
}

[Serializable]
public record BonusAreaEffect: StatChangerSource<AreaEffect>
{
    public BonusAreaEffect(): base()
    {
        sign = +1;
    }

    public BonusAreaEffect(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime)
    {
        sign = +1;
    }
}


[Serializable]
public record HealthUp : StatChangerSource<HealthPoint>
{
    public HealthUp(): base()
    {
        sign = +1;
    }

    public HealthUp(float value = 0, float duration = 0, Calculator calculator = Calculator.MaxPercent, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime)
    {
        sign = +1;
    }
}

[Serializable]
public record ATKInscrease: StatChangerSource<AttackDamage>
{
    public ATKInscrease(): base()
    {
        sign = +1;
    }

    public ATKInscrease(float value = 0, float duration = 0, Calculator calculator = Calculator.FloorPercent, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime)
    {
        sign = +1;
    }
}

[Serializable]
public record SizeUp : StatChangerSource<Area>
{
    public SizeUp(): base()
    {
        sign = +1;
    }

    public SizeUp(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime)
    {
        sign = +1;
    }
}

[Serializable]
public record Haste: StatChangerSource<MovementSpeed>
{
    public Haste(): base()
    {
        sign = +1;
    }

    public Haste(float value = 0, float duration = 0, Calculator calculator = Calculator.MaxPercent, int immuneStack = 1) : base(value, duration, calculator, immuneStack)
    {
        sign = +1;
    }
}

[Serializable]
public record Slow : StatChangerSource<MovementSpeed>
{
    public Slow(): base()
    {
        sign = -1;
    }

    public Slow(float value = 0, float duration = 0, Calculator calculator = Calculator.MaxPercent, int immuneStack = 1) : base(value, duration, calculator, immuneStack) 
    {
        sign = -1;
    }
}

[Serializable]
public record Windfury : StatChangerSource<TimerSpeed>
{
    public Windfury(): base()
    {
        sign = +1;
    }

    public Windfury(float value = 0, float duration = 0, Calculator calculator = Calculator.FloorPercent, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime) 
    {
        sign = +1;
    }
}

[Serializable]
public record Cripple: StatChangerSource<TimerSpeed>
{
    public Cripple(): base()
    {
        sign = -1;
    }

    public Cripple(float value = 0, float duration = 0, Calculator calculator = Calculator.MaxPercent, int immuneStack = 1, float refreshTime = StatSystem.CONSTANT_DURATION) : base(value, duration, calculator, immuneStack, refreshTime)
    {
        sign = -1;
    }
}

[Serializable]
public record Healing : StatChangerSource<HealthPoint>
{
    public Healing(): base()
    {
        sign = +1;
    }

    public Healing(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1) : base(value, duration, calculator, immuneStack) 
    {
        sign = +1;
    }
}

[Serializable]
public record Damage : StatChangerSource<HealthPoint>
{
    public bool isCritical { get; set; }
    public float criticalDamage { get; set; }

    public Damage(): base()
    {
        sign = -1;
    }

    public Damage(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1, float refreshTime = 5) : base(value, duration, calculator, immuneStack, refreshTime) 
    {
        sign = -1;
    }

    public Damage(Damage original): base(original)
    {
        isCritical = original.isCritical;
        criticalDamage = original.criticalDamage;
    }

    public override void ModifyValue(IEffectable target, ref float changedValue)
    {
        if (target.TryToGetStat(out Armor armor))
        {
            changedValue *= 1 - armor.damageReduction;
        }
    }

    public override IStatEffect Serialize()
    {
        return new Damage(this);
    }
}

[Serializable]
public record CooldownReduction : StatChangerSource<Stat>
{
    public CooldownReduction() : base()
    {
        sign = -1;
    }

    public CooldownReduction(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1) :base (value, duration, calculator, immuneStack) 
    {
        sign = -1;
    }
}

public interface IStatEffect: IIdentifiedData
{
    IEffectOrigin origin { get; set; }

    float value { get; set; }

    IStatEffect Serialize();

    float Apply(IEffectable statHolder);

    void Set(IEffectable statHolder);

    void Shrink(IEffectable statHolder);

    bool Merge(IStatEffect bonusEffect);
}

public record KnockBack : CrowdControlEffect
{
    public KnockBack() : base()
    {

    }

    public KnockBack(float duration) : base(duration)
    {

    }
}

[Serializable]
public record CrowdControlEffect : IStatEffect
{
    [field:SerializeField] public IEffectOrigin origin { get; set; }

    [field:SerializeField] public float value { get; set; }

    [field:SerializeField] public float duration { get; set; }

    public int ID => throw new NotImplementedException();

    public string Name => throw new NotImplementedException();

    public string Description => throw new NotImplementedException();

    public CrowdControlEffect()
    {
        this.duration = 0;
    }

    public CrowdControlEffect(float duration)
    {
        this.duration = duration;
    }

    public virtual float Apply(IEffectable target)
    {
        return 0;
    }

    public IStatEffect Serialize()
    {
        return new CrowdControlEffect(duration);
    }

    public bool Merge(IStatEffect bonusEffect)
    {
        throw new NotImplementedException();
    }

    public void Set(IEffectable statHolder)
    {
        throw new NotImplementedException();
    }

    public void Shrink(IEffectable statHolder)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public record StatChangerSource<T>: IStatEffect where T : Stat
{
    public enum Calculator
    {
        Direct,
        MaxPercent,
        CurrentPercent,
        FloorPercent,
        MissingPercent
    }

    /*[field: ReadOnly][field: SerializeField]*/
    public int ID { get; private set; }
    [field: SerializeField] public float value { get; set; }
    [field:ReadOnly] [field: SerializeField] public int sign { get; protected set; }
    public float decayTime { get; private set; }
    [field:SerializeField] public Calculator calculator { get; private set; }

    [field: SerializeReference] public IEffectOrigin origin { get; set; }

    public int immuneStack { get; set; }
    public float refreshTime { get; set; }

    public string Name => throw new NotImplementedException();

    public string Description => throw new NotImplementedException();

    public virtual IStatEffect Serialize()
    {
        return new StatChangerSource<T>(this);
    }

    public bool Merge(IStatEffect bonusEffect)
    {
        if (bonusEffect is StatChangerSource<T> sameEffect)
        {
            value += sameEffect.value;
            return true;
        }
        return false;
    }

    public float Apply(IEffectable target)
    {
        if (target is IStat<T> statHolder)
        {
            //Debug.Log(typeof(T)); 
            var stat = statHolder.GetStat();
            //Debug.Log(value); 
            var changedValue = GetDirectValue(stat);
            ModifyValue(target, ref changedValue);
            //Debug.Log(sign);
            //Debug.Log(stat.max); 
            //Debug.Log(changedValue); 
            //Debug.Log(changedValue / stat.max);           
            
            var res = stat.Change(changedValue);

            if (statHolder is IThreshold<T> threshold)
            {
                //if (typeof(T).Equals(typeof(Energy)))
                //{
                //    Debug.Log(stat.current + "-" + stat.max); 
                //}
                if (stat.current == 0) threshold.OnMin?.Invoke();
                else if (stat.current == stat.max) threshold.OnMax?.Invoke();
            }
            //Debug.Log(statHolder.OnStatChange.GetInvocationList().Length); 
            statHolder.OnStatEffected?.Invoke(stat.current, stat.max);
            return res;

        }
        else return 0;
    }

    public void Set(IEffectable target)
    {
        if (target is IStat<T> statHolder)
        {
            Stat stat = statHolder.GetStat();
            var res = stat.Change(value - stat.current);

            if (statHolder is IThreshold<T> threshold)
            {
                //if (typeof(T).Equals(typeof(Energy)))
                //{
                //    Debug.Log(stat.current + "-" + stat.max); 
                //}
                if (stat.current == 0) threshold.OnMin?.Invoke();
                else if (stat.current == stat.max) threshold.OnMax?.Invoke();
            }
            //Debug.Log(statHolder.OnStatChange.GetInvocationList().Length); 
            statHolder.OnStatEffected?.Invoke(stat.current, stat.max);
        }
    }

    public void Shrink(IEffectable target)
    {
        if (target is IStat<T> statHolder)
        {
            Stat stat = statHolder.GetStat();
        }
    }

    public float GetDirectValue(T stat)
    {
        switch (calculator)
        {
            default: return value * sign;

            case Calculator.MaxPercent: return value * stat.max * sign;

            case Calculator.CurrentPercent : return value * stat.current * sign;

            case Calculator.FloorPercent: return value * stat.floor * sign;

            case Calculator.MissingPercent: return value * (stat.max - stat.current) * sign;
        }
    }

    public virtual void ModifyValue(IEffectable target, ref float changedValue)
    {
        
    }

    public void Refesh(float value)
    {
        ID = StatSystem.ID_COUNTER++;
        this.value = value;
    }

    public float Modify_Obsolete(IEffectable target)
    {
        if (target is IStat<T> statHolder)
        {
            Stat stat = statHolder.GetStat();
            switch (calculator)
            {
                case Calculator.Direct:
                    stat += value * sign;

                    break;
                case Calculator.MaxPercent:
                    stat ^= value * sign;

                    break;
                case Calculator.FloorPercent:

                    stat.SetFloor(stat.floor * (1 + value * sign));
                    break;
            }
        }

        return 0;
    }

    public StatChangerSource()
    {
        this.ID = StatSystem.ID_COUNTER++;
        this.immuneStack = 1;
        this.value = 1;
        this.decayTime = 0;
        this.calculator = Calculator.Direct;
        this.refreshTime = 5;
    }

    public StatChangerSource(float value)
    {
        this.ID = StatSystem.ID_COUNTER++;
        this.immuneStack = 1;
        this.value = value;
        this.decayTime = 0;
        this.calculator = Calculator.Direct;
        this.refreshTime = 5;
    }

    public StatChangerSource(float value, float duration)
    {
        this.ID = StatSystem.ID_COUNTER++;
        this.immuneStack = 1;
        this.value = value;
        this.decayTime = duration;
        this.calculator = Calculator.Direct;
        this.refreshTime = 5;
    }

    public StatChangerSource(float value, float duration, Calculator calculator)
    {
        this.ID = StatSystem.ID_COUNTER++;
        this.immuneStack = 1;
        this.value = value;
        this.decayTime = duration;
        this.calculator = calculator;
        this.refreshTime = 5;
    }

    public StatChangerSource(float value = 0, float duration = 0, Calculator calculator = Calculator.Direct, int immuneStack = 1, float refreshTime = 5, int collideMask = Layer.ENEMY_MASK)
    {
        this.ID = StatSystem.ID_COUNTER++;
        this.immuneStack = immuneStack;
        this.value = value;
        this.decayTime = duration;
        this.calculator = calculator;
        this.refreshTime = refreshTime;
    }

    public StatChangerSource(StatChangerSource<T> original)
    {
        ID = StatSystem.ID_COUNTER++;
        value = original.value;
        sign = original.sign;
        decayTime = original.decayTime;
        calculator = original.calculator;
        immuneStack = original.immuneStack;
        refreshTime = original.refreshTime;
    }
}

