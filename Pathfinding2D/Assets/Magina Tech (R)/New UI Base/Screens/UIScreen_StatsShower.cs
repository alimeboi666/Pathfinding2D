using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Hung.GameData;
using Hung.StatSystem;
using Hung.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;
using static Hung.StatSystem.Stat;

public sealed class UIScreen_StatsShower : UIScreen
{
    [field:SerializeField] public IModifiedEffectable visualizeTarget { get; private set; }
    [SerializeField] private UI_StatSlot firstSlot;
    [SerializeField] private TMP_Text txtStatName;
    [SerializeField] private TMP_Text txtTotalValue;
    [SerializeField] private List<UI_StatSlot> slots;

    [NonSerialized, OdinSerialize] Dictionary<IEffectOrigin, Modifier> boosters;

    [ReadOnly, SerializeField] private Modifier otherModifier;

    void Awake()
    {
        
    }

    public void SetVisual<T>(string statVisualName, IModifiedEffectable effectable, T stat, ModifyCalculator calculator) where T: Stat
    {
        boosters = new();

        visualizeTarget = effectable;
        //Debug.Log("Stat type: " + typeof(T).Name);

        txtStatName.text = $"{statVisualName}'s {Regex.Replace(calculator.ToString(), "(?<!^)([A-Z])", " $1")}";

        foreach (var effect in effectable.effects)
        {
            if (effect.Value is StatModifierSource<T> modifier)
            {
                //Debug.Log("Stat Modifier Source: " +  typeof(T).Name); 
                modifier.CreatePreset();
                if (boosters.ContainsKey(effect.Value.origin))
                {
                    boosters[effect.Value.origin] &= modifier.preset;
                }
                else
                {
                    boosters[effect.Value.origin] = modifier.preset;
                }            
            }            
        }

        //foreach(var booster in boosters)
        //{
        //    var description = booster.Key.Description;
        //    slots[i++].Set($"{booster.Key.Name}" + description != "" ? $"(from {booster.Key.Description})" : "", booster.Value);
        //    otherModifier &= booster.Value;
        //}

        UIItemUtilities.Match(boosters, slots, (booster, slot) =>
        {
            var description = booster.Key.Description;
            if (description.Split(' ').Length > 4) description = "";
            slot.Set($"{booster.Key.Name}" + (description != "" ? $" (from {description})" : ""), booster.Value);
            otherModifier &= booster.Value;
        });


        if (calculator == ModifyCalculator.BonusPercent)
        {
            var percent = Mathf.RoundToInt(((stat.modifier.percent * stat.modifier.multiplier - 1) * 100));

            txtTotalValue.text = $"{(percent >= 0 ? "+" : "")}{percent}%";
        }
        else if (calculator == ModifyCalculator.Multiple)
        {
            txtTotalValue.text = $"x{stat.modifier.multiplier:0.##}";
        }

        otherModifier = stat.modifier - otherModifier;
    }

}
