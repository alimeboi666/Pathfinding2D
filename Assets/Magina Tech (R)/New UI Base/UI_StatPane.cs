using System;
using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using Hung.GameData.RPG;
using Hung.StatSystem;
using Hung.UI.Button;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Hung.StatSystem.Stat;

public enum StatInfoType
{
    RPGValue,
    Modifier
}

public class UI_StatPane : MonoBehaviour
{
    private IStat statHolder;
    [field: SerializeField] public string visualName { get; private set; }
    [SerializeField] private UnitStat bindingStat;
    [SerializeField] private StatInfoType visualType;
    [ShowIf("visualType", Value = StatInfoType.Modifier), SerializeField] private ModifyCalculator calculator;

    [SerializeField] private TMP_Text txtValue;

    Stat targetStat;
    Action showStatAction;

    private void Awake()
    {
        statHolder = Archetype.Player;
        if (statHolder != null) SetVisual(statHolder);
        if (TryGetComponent(out SimpleButton btn))
        {
            btn.ClickEvent.AddListener(ShowCurrentInfo);
        }
    }

    void ShowCurrentInfo()
    {
        showStatAction?.Invoke();
    }

    void ShowStatInfo<T>(T stat) where T : Stat
    {
        if (statHolder != null && statHolder is IModifiedEffectable effectable)
        {
            var screen = Archetype.UIManager.GetScreen<UIScreen_StatsShower>();
            screen.VisualOn();
            screen.SetVisual(visualName, effectable, stat, calculator);
        }       
    }

    public void SetVisual(IStat statHolder)
    {
        this.statHolder = statHolder;
        switch (bindingStat)
        {
            case UnitStat.MS:
                if (statHolder.TryToGetStat(out MovementSpeed ms))
                {
                    ms.OnModifierChange += (Modifier modifier) => UpdateModifierValue();
                    targetStat = ms;

                    showStatAction = () => ShowStatInfo(ms);
                }
                break;

            case UnitStat.Amount:
                if (statHolder.TryToGetStat(out Amount amount))
                {
                    amount.OnModifierChange += (Modifier modifier) => UpdateModifierValue();
                    targetStat = amount;

                    showStatAction = () => ShowStatInfo(amount);
                }
                break;
        }       
    }

    void UpdateModifierValue()
    {

        if (calculator == ModifyCalculator.BonusPercent)
        {
            var percent = Mathf.RoundToInt(((targetStat.modifier.percent * targetStat.modifier.multiplier - 1) * 100));

            txtValue.text = $"{(percent >= 0 ? "+" : "")}{percent}%";
            if (percent > 0)
            {
                txtValue.color = Color.yellow;
            }
            else if (percent < 0)
            {
                txtValue.color = Color.red;
            }
            else txtValue.color = Color.white;
        }
        else if (calculator == ModifyCalculator.Multiple)
        {
            txtValue.text = $"x{targetStat.modifier.multiplier:0.##}";
            if (targetStat.modifier.multiplier > 1)
            {
                txtValue.color = Color.yellow;
            }
            else if (targetStat.modifier.multiplier < 1)
            {
                txtValue.color = Color.red;
            }
            else txtValue.color = Color.white;
        }
    }
}
