
using System;
using UnityEngine;

public interface ICast<T> where T: IFormattable
{
    void Cast(T input);

    void Precast();
}

public interface IComplexCast<T>
{
    void Cast(T input, Action callback = null);

    void Precast();
}

public interface IComplexCast<T, U>
{
    void Cast(T input, U input2, Action callback = null);

    void Precast();
}

public interface ICast<T1, T2> where T1: IFormattable
{
    void Cast(T1 input1, T2 input2);

    void Precast();
}

public interface ICast: IMono, IToggleable
{
    void Cast();

    void Dispel();
}

public interface IToggleable: IMono
{
    event Action<bool> OnVisualChanged;

    bool isVisible { get; }

    void ToggleOn();

    void ToggleOff();
}