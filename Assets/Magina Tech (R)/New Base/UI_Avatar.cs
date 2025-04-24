using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public record Avatar
{
    public Sprite icon;
    public Color color;
    public Color colorContrast;

    public Avatar(Sprite icon, Color color, Color colorConstract = default)
    {
        this.icon = icon;
        this.color = color;
        this.colorContrast = colorConstract;
    }
}

public class UI_Avatar : MonoBehaviour, IToggleable
{
    [SerializeField] Image icon;
    [SerializeField] Image background;
    [SerializeField] Image contrast;

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public event System.Action<bool> OnVisualChanged;

    public void SetAvatar(Avatar avatar)
    {
        icon.sprite = avatar.icon;
        icon.Normalize();
        background.color = avatar.color;
        if (contrast != null) contrast.color = avatar.colorContrast;
    }

    public void SetAvatar(Sprite sprite, Color colorBG)
    {
        icon.sprite = sprite;
        icon.Normalize();
        background.color = colorBG;
    }

    public void SetAvatar(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.Normalize();
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
}
