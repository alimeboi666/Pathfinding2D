using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Hung.Pooling;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Value : MonoBehaviour, IPoolable
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text txtValue;
    [SerializeField] private CanvasGroup canvasGroup;

    [field: ReadOnly, HideInEditorMode, SerializeField] public string value { get; private set; }
    [field: ReadOnly, HideInEditorMode, SerializeField] public string suffix { get; private set; }  

    public PoolPartition pooling { get; set; }
    public bool isNewPooling { get; set; }

    public TMP_Text GetText() => txtValue;

    public void SetType(Sprite iconSprite, string suffix = "")
    {
        icon.sprite = iconSprite;
        this.suffix = suffix;
    }

    public void SetValue(string value, bool hasSpace = false)
    {
        this.value = value;
        txtValue.text = value + (hasSpace ? " " : "") + suffix;
    }

    public void SetSuffix(string suffix = "")
    {
        this.suffix = suffix;
    }

    public void SetValueBonus(string value, bool hasSpace = false)
    {
        this.value = value;
        txtValue.text = "+" + value + (hasSpace ? " " : "") + suffix;
    }

    public TweenerCore<float, float, FloatOptions> DOFade(float duration)
    {
        canvasGroup.DOKill();
        canvasGroup.alpha = 1.0f;
        return canvasGroup.DOFade(0, duration);
    }

    public bool enableText
    {
        get => txtValue.gameObject.activeSelf;

        set => txtValue.gameObject.SetActive(value);
    }
    
}
