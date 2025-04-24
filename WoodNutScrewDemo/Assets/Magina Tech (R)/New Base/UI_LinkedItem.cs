using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class UI_LinkedItem : SubControl<UI_CollapseList>, IModel, IToggleable
{
    [field: SerializeField] public GameObject Model { get; private set; }

    [field: SerializeField] public UI_LinkedItem above { get; set; }
    [field: SerializeField] public UI_LinkedItem below { get; set; }

    public RectTransform rect => transform as RectTransform;

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public event Action<bool> OnVisualChanged;

    public void ToggleOn()
    {
        OnVisualChanged?.Invoke(true);
        isVisible = true;
        Model.SetActive(true);
    }

    public void ToggleOff()
    {
        OnVisualChanged?.Invoke(false);
        isVisible = false;
        Model.SetActive(false);
    }
}
