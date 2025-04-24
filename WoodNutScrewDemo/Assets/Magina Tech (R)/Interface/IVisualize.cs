using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisualize
{
    event Action OnClosed;

    void VisualOn();

    void VisualOff();
}
