using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum SettingOption
{
    Music,
    Sound,
    Vibration
}

public enum Switch
{
    On = 1,
    Off = 0   
}

public static class Switcher
{
    public static Switch Toggle(this Switch @switch)
    {
        return @switch == Switch.On ? Switch.Off : Switch.On;
    }
}

public sealed class SettingEvent : UI_EventHolder
{
    [SerializeField] private RectTransform expandingRect;
    private Switch[] m_settings;
    private UIShower[] m_switchShowers;

    private Vector2 collapseSize, expandSize;

    [SerializeField] private float tweeningSpeed;

    protected override void Awake()
    {
        base.Awake();

        int n = transform.childCount;
        SettingOption _option;
        m_settings = new Switch[n];
        m_switchShowers = new UIShower[n];
        for (int i = 0; i < n; i++)
        {
            _option = (SettingOption)i;
            m_switchShowers[i] = transform.GetChild(i).GetComponent<UIShower>();
            Core_Switch(_option, PlayerPersistentData.Get_SettingOption(_option));
        }


        expandSize = expandingRect.sizeDelta;
        collapseSize = new Vector2(expandingRect.sizeDelta.x, 0);

        expandingRect.sizeDelta = collapseSize;
        isExpanding = false;
    }


    [SerializeField] public bool isExpanding;
    public void VisualExpand()
    {
        if (!isExpanding)
        {
            isExpanding = true;
            expandingRect.DOSizeDelta(expandSize, tweeningSpeed).SetEase(Ease.OutBack);
        }
        else
        {
            isExpanding = false;
            expandingRect.DOSizeDelta(collapseSize, tweeningSpeed * 0.65f).SetEase(Ease.InBack);
        }        
    }

    int _index;
    protected override bool ClickAction(PointerEventData eventData, GameObject clicked)
    {
        _index = clicked.transform.GetSiblingIndex();

        OnBlindSwitch(_index);

        return true;
    }

    private void Core_Switch(SettingOption settingOption, Switch @switch)
    {
        m_settings[(int)settingOption] = @switch;

        SwitchAt((int)settingOption, @switch);
    }

    private void OnBlindSwitch(int index)
    {
        Core_Switch((SettingOption)index, m_settings[index].Toggle());

        PlayerPersistentData.Set_SettingOption((SettingOption)index, m_settings[index]);
    }

    private void SwitchAt(int index, Switch @switch)
    {
        m_switchShowers[index].SingleShow(@switch);
    }
}
