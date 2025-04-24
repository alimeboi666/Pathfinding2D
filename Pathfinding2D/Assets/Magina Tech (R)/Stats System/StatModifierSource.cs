using System.Collections;
using System.Collections.Generic;
using Hung.GameData.RPG;
using Hung.Unit;
using Sirenix.OdinInspector;
using UnityEngine;
using static Hung.StatSystem.Stat;

namespace Hung.StatSystem
{
    public record QuantityModifier: StatModifierSource<Quantity>
    {
        public QuantityModifier(float value) : base(value, ModifyCalculator.Flat)
        {

        }
    }

    public record MultiplierAmount: StatModifierSource<Amount>
    {
        public MultiplierAmount(): base(0, ModifyCalculator.MultiplierPercent)
        {

        }

        public MultiplierAmount(float value): base(value, ModifyCalculator.MultiplierPercent)
        {

        }
    }

    public record GlobalSpeed: StatModifierSource<MovementSpeed>
    {
        public GlobalSpeed(): base(0, ModifyCalculator.MultiplierPercent)
        {

        }

        public GlobalSpeed(float value, ModifyCalculator calculator = ModifyCalculator.MultiplierPercent): base(value, calculator)
        {

        }
    }

    public record BonusSpeed: StatModifierSource<MovementSpeed>
    {
        public BonusSpeed(float value): base(value, ModifyCalculator.BonusPercent) 
        {

        }
    }

    public enum ModifyCalculator
    {
        Flat,
        BonusPercent,
        MultiplierPercent,
        Multiple
    }

    public record StatModifierSource<T> : IStatEffect where T : Stat
    {
        [field: ReadOnly, SerializeField] public int ID { get; private set; }

        [field: SerializeReference] public IEffectOrigin origin { get; set; }
        public float value { get; set; }

        public Modifier preset { get; protected set; }

        public bool onlyMax { get; set; }

        [field: SerializeField] public ModifyCalculator calculator { get; private set; }

        public string Name => throw new System.NotImplementedException();

        public string Description => throw new System.NotImplementedException();

        public void CreatePreset()
        {
            preset = new();
            switch (calculator)
            {
                case ModifyCalculator.Flat: preset += value; break;

                case ModifyCalculator.BonusPercent: preset ^= value; break;

                case ModifyCalculator.MultiplierPercent: preset *= 1 + value; break;

                case ModifyCalculator.Multiple: preset *= value; break;
            }
        }


        public StatModifierSource(float value, ModifyCalculator calculator)
        {
            ID = StatSystem.ID_COUNTER++;
            this.value = value;
            this.calculator = calculator;
        }

        public float Apply(IEffectable target)
        {
            if (target is IStat<T> modifyHolder)
            {              
                Stat stat = modifyHolder.GetStat();
                var pre = stat.max;
                if (origin != null && target is IModifiedEffectable modified)
                {
                    if (!modified.effects.ContainsKey(ID)) modified.effects[ID] = this;
                    else return 0;
                }
                switch (calculator)
                {
                    case ModifyCalculator.Flat:
                        
                        if (value > 0 && onlyMax) 
                        {
                            stat.MaxPlus(value);
                        }
                        else
                        {
                            stat += value;
                        }
                        break;

                    case ModifyCalculator.BonusPercent: stat ^= value; break;

                    case ModifyCalculator.MultiplierPercent: stat *= 1 + value; break;

                    case ModifyCalculator.Multiple: stat *= value; break;
                }
                modifyHolder.OnStatEffected?.Invoke(stat.current, stat.max);
                return stat.max - pre;
            }
            return 0;
        }
        public void Set(IEffectable target)
        {
            //if (target is IStat<T> modifyHolder)
            //{
            //    Stat stat = modifyHolder.GetStat();

            //    switch (calculator)
            //    {
            //        case Calculator.Flat: stat += value; break;

            //        case Calculator.BonusPercent: stat ^= value; break;

            //        case Calculator.MultiplierPercent: stat *= value; break;
            //    }
            //}
        }

        public float Revoke(IEffectable target)
        {
            if (target is IStat<T> modifyHolder)
            {
                Stat stat = modifyHolder.GetStat();
                if (target is IModifiedEffectable modified)
                {
                    modified.effects.Remove(ID);
                }

                switch (calculator)
                {
                    case ModifyCalculator.Flat: stat -= value; break;

                    case ModifyCalculator.BonusPercent: stat ^= -value; break;

                    case ModifyCalculator.MultiplierPercent: stat /= 1 + value; break;

                    case ModifyCalculator.Multiple: stat /= value; break;
                }
            }
            return 0;
        }

        public void Shrink(IEffectable statHolder)
        {
            throw new System.NotImplementedException();
        }

        public bool Merge(IStatEffect bonusEffect)
        {
            throw new System.NotImplementedException();
        }

        public IStatEffect Serialize()
        {
            throw new System.NotImplementedException();
        }


    }
}
