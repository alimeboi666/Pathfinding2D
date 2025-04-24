using System.Collections;
using System.Collections.Generic;
using Hung.Core;
using Hung.GameData;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Hung/Data/Setting")]
public class SettingData : GameData
{
    [SerializeField] private SettingExecute Execute;

    public override void Init()
    {
        base.Init();
        //Debug.Log("Setting: " + name + " is init");
        Execute.Init();
        Execute.Change(isOn);
    }

    public bool isOn
    {
        get => PlayerPrefs.GetInt(name, 1) == 1;

        set
        {
            //Debug.Log("Set setting of " + Name + " to: " + value);
            Execute.Change(value);
            PlayerPrefs.SetInt(name, value ? 1 : 0);
        }
    }
}

public class SoundSetting : SettingExecute
{
    public override void Change(bool isOn)
    {
        Debug.Log("SOUND setting: " + isOn);
        Archetype.MasterSound.isSoundOn = isOn;
    }
}

public class BGMusicSetting: SettingExecute
{
    public override void Change(bool isOn)
    {
        Archetype.MasterSound.isMusicOn = isOn;
        
    }
}

public class VibrationSetting: SettingExecute
{
    public override void Change(bool isOn)
    {
        //SingletonRole.MasterHaptic.isHapticOn = isOn;
    }
}

public abstract class SettingExecute : ICore
{
    public void Init()
    {
        
    }

    public abstract void Change(bool isOn);
}