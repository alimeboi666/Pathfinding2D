using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INotiCast: IMono
{
    bool hasNotify { get; }

    event Action<bool> Notify;

    void PostNoti(bool isOn);
}

public interface INotiReceive: IMono
{
    RectTransform redNoti { get; }

    List<INotiCast> checkers { get; }
}