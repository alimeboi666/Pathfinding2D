using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingOptionToggler<T> where T : ISettingOption
{
    void Register(T soundPlayer);

    void Remove(T soundPlayer);
}

public interface IPlaySound<T>: IPlaySound where T: System.Enum
{
    void PlaySound(T soundType);
}

public interface ISettingOption
{

}

public interface IPlaySound: ISettingOption
{
    AudioSource audioSource { get; }

    void ToggleSound(bool isOn);
}

public interface IPlayShake : ISettingOption
{
    void ToggleShake(bool isOn);
}