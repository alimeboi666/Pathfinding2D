using System;
using Hung.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class UITooltip : MonoBehaviour, IToggleable
{
    [SerializeField] private TMP_Text txtDes;
    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public event Action<bool> OnVisualChanged;

    public void Set(string content)
    {
        txtDes.text = content;
    }

    public void ToggleOff()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }

    public void ToggleOn()
    {
        isVisible = true;
        gameObject.SetActive(true);
    }

    void OnEmptyClicked(Vector2 screenPos)
    {
        ToggleOff();
    }

    private void OnEnable()
    {
        Archetype.ScreenListener.OnEmptyClick += OnEmptyClicked;
    }

    private void OnDisable()
    {
        Archetype.ScreenListener.OnEmptyClick -= OnEmptyClicked;
    }
}
