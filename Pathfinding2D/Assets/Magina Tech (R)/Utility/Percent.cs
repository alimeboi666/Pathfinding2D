using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable, InlineProperty]
public struct Percent
{
    [HideLabel, Range(0, 100)] public float value;

    public float mathValue => value / 100;

    public bool Roll()
    {
        return Random.Range(0, 100) <= value;
    }

    public static bool Roll(float percent)
    {
        return Random.Range(0, 1f) <= percent;
    }

    public Percent(float percentValue)
    {
        if (percentValue < 0 || percentValue > 1)
            throw new ArgumentOutOfRangeException(nameof(percentValue), "Value must be between 0 and 100.");
        this.value = percentValue * 100;
    }

    public static implicit operator Percent(float value)
    {
        return new Percent(value);
    }

    public static explicit operator float(Percent percent)
    {
        return percent.value / 100;
    }

    public override string ToString()
    {
        return $"{value}%";
    }
    /// <summary>
    /// Get a random float value between 0 and 1
    /// </summary>
    /// <returns></returns>
    public static float GetRandomPercent()
    {
        return Random.Range(0, 100) / 100;
    }
}