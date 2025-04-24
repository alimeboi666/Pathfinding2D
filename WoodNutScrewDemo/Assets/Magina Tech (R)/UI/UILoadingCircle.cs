using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingCircle : MonoBehaviour, IProcess
{
    [SerializeField] private Image render;
    public float progress
    {
        get => render.fillAmount;

        set => render.fillAmount = value;

    }

    public void ToggleOff()
    {
        
    }

    public void ToggleOn()
    {
        
    }

    public void VisualChange(float toValue, float duration, Action endCallback = null)
    {
        render.DOFillAmount(toValue, duration).OnComplete(() =>
        {
            endCallback?.Invoke();
        });
    }

    private void OnDisable()
    {
        render.DOKill();
    }
}

