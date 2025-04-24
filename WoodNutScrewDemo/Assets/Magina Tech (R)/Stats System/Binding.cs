using Hung.GameData.RPG;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;
using UnityExtensions;


namespace Hung.StatSystem.Binding
{
    public abstract class BindingStat<Key, Value> : IStat where Key : System.Enum
    {
        [HideLabel][SerializeField] private Dictionary<Key, Value> stats;

        public bool TryToGetValue(Key stat, out Value value)
        {
            if (stats.ContainsKey(stat))
            {
                value = stats[stat];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void TryToChangeValue(Key stat, Value value)
        {
            if (stats.ContainsKey(stat))
            {
                stats[stat] = value;
            }
            else
            {
                stats.Add(stat, value);
            }
        }

        public void BuildStats(IStat target)
        {
            foreach (var statPair in GetAll())
            {
                ConvertStat(target, statPair.Key, statPair.Value);
            }
        }

        protected abstract void ConvertStat(IStat target, Key statType, Value value);

        public List<KeyValuePair<Key, Value>> GetAll()
        {
            return DictionaryToList(stats);
        }

        static List<KeyValuePair<Key, Value>> DictionaryToList(Dictionary<Key, Value> dictionary)
        {
            List<KeyValuePair<Key, Value>> list = new List<KeyValuePair<Key, Value>>();

            foreach (KeyValuePair<Key, Value> kvp in dictionary)
            {
                list.Add(kvp);
            }

            return list;
        }
    }

    public class ReferenceStat<T> : IStat where T : IStat
    {
        [HideLabel, SerializeField] private Dictionary<T, float> stats;

        public float TryToGetValue(T stat)
        {
            if (stats.ContainsKey(stat))
            {
                return stats[stat];
            }
            else return -1;
        }

        public List<KeyValuePair<T, float>> GetAll()
        {
            return DictionaryToList(stats);
        }

        static List<KeyValuePair<T, float>> DictionaryToList(Dictionary<T, float> dictionary)
        {
            List<KeyValuePair<T, float>> list = new List<KeyValuePair<T, float>>();

            foreach (KeyValuePair<T, float> kvp in dictionary)
            {
                list.Add(kvp);
            }

            return list;
        }
    }


    public class UnitBindingStat : BindingStat<UnitStat, float>
    {
        protected override void ConvertStat(IStat target, UnitStat statType, float value)
        {
            switch (statType)
            {
                case UnitStat.Price:
                    if (target.TryToGetStat(out Price price))
                    {
                        price.SetValue(value);
                    }

                    break;

                case UnitStat.Level:
                    if (target.TryToGetStat(out Level lvl))
                    {
                        lvl.SetValue(value);
                        lvl.Empty();
                    }

                    break;

                case UnitStat.Amount:
                    if (target.TryToGetStat(out Amount amount))
                    {
                        amount.SetValue(value);
                    }

                    break;

                case UnitStat.Cooldown:
                    if (target.TryToGetStat(out Cooldown cd))
                    {
                        cd.SetValue(value);
                    }

                    break;

                case UnitStat.Fury:
                    if (target.TryToGetStat(out Fury fury))
                    {
                        fury.SetValue(value);
                        fury.Empty();
                    }
                    break;

                case UnitStat.Quantity:
                    if (target.TryToGetStat(out Quantity quantity))
                    {
                        quantity.SetValue(value);
                        quantity.Empty();
                    }
                    break;

                case UnitStat.MS:
                    if (target.TryToGetStat(out MovementSpeed ms))
                    {
                        ms.SetFloor(value);
                    }
                    break;
            }
        }
    }

    public class AbilityBindingStat : BindingStat<AbilityStat, float>
    {
        protected override void ConvertStat(IStat target, AbilityStat statType, float value)
        {
            throw new NotImplementedException();
        }
    }

    public struct StatValue<Value>
    {
        [HideLabel, HorizontalGroup(LabelWidth = 40)][ValueDropdown("GetRPGStatTypes")] public Type statType;
        [HideLabel, HorizontalGroup(LabelWidth = 40)] public Value value;

        IEnumerable<Type> GetRPGStatTypes()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var derivedClasses = assembly.GetTypes()
                    .Where(type => type.IsClass && type.IsSubclassOf(typeof(Stat))).Reverse();
            return derivedClasses.Select(type => type);
        }

    }

    public abstract class Statistic<Value>
    {
        [ReadOnly, ShowInInspector] private Dictionary<Type, Value> stats = new Dictionary<Type, Value>();

        [HideLabel][SerializeField] List<StatValue<Value>> statList = new();

        [Button]
        public void Init()
        {
            if (stats != null) stats.Clear();
            else stats = new();

            foreach (var stat in statList)
            {
                if (!stats.ContainsKey(stat.statType))
                {
                    stats.Add(stat.statType, stat.value);
                }
                else
                {
                    Debug.LogWarning($"Type {stat.statType} already exists in the dictionary.");
                }
            }
        }

        public bool TryToGetValue<T>(out Value value) where T : Stat
        {
            Type type = typeof(T);
            if (stats.ContainsKey(type))
            {
                value = stats[type];
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void TryToChangeValue<T>(Value value) where T : Stat
        {
            Type type = typeof(T);
            if (stats.ContainsKey(type))
            {
                stats[type] = value;
            }
            else
            {
                stats.Add(type, value);
            }
        }

        public void BuildStats(IStat target)
        {
            foreach (var stat in stats)
            {
                Type iStatType = typeof(IStat<>).MakeGenericType(stat.Key);

                if (iStatType.IsInstanceOfType(target))
                {
                    //Debug.Log($"Unit implements IStat<{stat.statType.Name}>");
                    var getStatMethod = iStatType.GetMethod("GetStat");
                    Stat statInfo = null;
                    if (getStatMethod != null)
                    {
                        statInfo = getStatMethod.Invoke(target, null) as Stat;
                    }
                    var rankingValuesProperty = iStatType.GetProperty("rankingValues");
                    float[] rankingValues = null;
                    if (rankingValuesProperty != null)
                    {
                        //Debug.Log("Got Ranking Values");
                        rankingValues = (float[])rankingValuesProperty.GetValue(target);

                    }

                    SetValue(target, statInfo, ref rankingValues, stat.Value);
                    if (rankingValuesProperty != null)
                    {
                        rankingValuesProperty.SetValue(target, rankingValues);
                    }
                }
            }
        }

        protected abstract void SetValue(IStat target, Stat stat, ref float[] rankingValues, Value value);
    }

    public struct RankingValue
    {
        [HorizontalGroup(LabelWidth = 40)] public float @base;
        [HorizontalGroup(LabelWidth = 50)] public float change;
    }

    public class RankableStatistic : Statistic<RankingValue>
    {
        protected override void SetValue(IStat target, Stat stat, ref float[] rankingValues, RankingValue value)
        {
            stat.SetValue(value.@base);
            if (target is ILevelable levelable)
            {
                int lvMax = (int)levelable.Level.max;
                Debug.Log("Level up: " + lvMax);
                if (rankingValues == null) rankingValues = new float[lvMax];
                else rankingValues.Resize(lvMax);
                for (int i = 0; i < lvMax; i++)
                {
                    rankingValues[i] = value.@base + value.change * i;
                }
                var copied = rankingValues.ToArray();
                levelable.Level.OnValueChange += (float lv) =>
                {
                    if (lv >= 1) stat.SetValue(copied[(int)lv - 1]);
                };
            }
        }
    }

    public class RPGValue_Statistic : Statistic<EnormousValue>
    {
        protected override void SetValue(IStat target, Stat stat, ref float[] rankingValues, EnormousValue value)
        {
            stat.SetValue(value);
        }
    }

    [Serializable]
    public struct ProgressionValue
    {
        public bool bonusUnstackable;
        public float bonus;
        public float percent;

        public int startFromRank;

        public ProgressionValue(float bonus, float percent, int startFromRank = 0, bool bonusStackable = false)
        {
            this.bonus = bonus;
            this.percent = percent;
            this.startFromRank = startFromRank;
            this.bonusUnstackable = bonusStackable;
        }

        public void GetValueByLevel(ref float baseValue, int level)
        {
            if (level == 0) return;
            if (level >= startFromRank)
            {
                if (!bonusUnstackable) baseValue = baseValue * Mathf.Pow(percent + 1, level) + (bonus * (level + 1) * level) / 2;
                else baseValue = baseValue * Mathf.Pow(percent + 1, level) + bonus * (level - 1);
            }
        }

        //public static implicit operator ProgressionValue(float value)
        //{
        //    return new ProgressionValue(value);
        //}
    }

    public class UnitBindingPercentableStat : BindingStat<UnitStat, ProgressionValue>
    {
        protected override void ConvertStat(IStat target, UnitStat statType, ProgressionValue value)
        {
            throw new NotImplementedException();
        }
    }

    public class UnitReferenceStat : ReferenceStat<IUnitStat>
    {

    }

    public enum AbilityStat : byte
    {
        Cooldown,
        Duration,
        Area,
        Speed,
        Quantity,
        Range
    }
}