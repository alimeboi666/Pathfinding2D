using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class UISubScreen : MonoBehaviour, IToggleable
{
    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public event Action<bool> OnVisualChanged;

    public void ToggleOff()
    {
        OnVisualChanged?.Invoke(false);
        gameObject.SetActive(false);
    }

    public void ToggleOn()
    {
        OnVisualChanged?.Invoke(true);
        gameObject.SetActive(true);       
    }

    private void OnEnable()
    {
        isVisible = true;
    }

    private void OnDisable()
    {
        isVisible = false;
    }
}
