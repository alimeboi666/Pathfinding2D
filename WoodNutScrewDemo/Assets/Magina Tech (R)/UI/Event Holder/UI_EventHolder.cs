using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_EventHolder<T>: UI_EventHolder where T: IUIInteractable
{
    protected T[] items;

    protected override bool ClickAction(PointerEventData eventData, GameObject clicked)
    {
        ClickItem(clicked.GetItem<T>());
        return true;
    }

    protected abstract void ClickItem(T item);
}

public abstract class UI_EventHolder : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
{
    protected const string TAG_FirtItem = "FirstItem";
    protected const string TAG_SecondItem = "SecondItem";
    protected const string TAG_ThirdItem = "ThirdItem";

    [SerializeField] private ClickType clickType;
    private float m_scale;

    protected float PopScale
    {
        get
        {
            if (m_scale == 0)
            {
                m_scale = clickType == ClickType.PopDown ? popDownScale : popUpScale;
            }
            return m_scale;
        }
    }
    protected const float popUpScale = 1.1f;

    protected const float popDownScale = 0.9f;

    protected GameObject holding;

    protected bool wasDragging;

    [SerializeField] private bool ignoreDragProctector;
    //[SerializeField] private ClickSoundType defaultClick; //, firstItemClick, secondItemClick, thirdItemClick;

    [field: ReadOnly, SerializeField] public bool isLock { get; set; }
    protected override void OnEnable()
    {
        isLock = false;
    }

    protected abstract bool ClickAction(PointerEventData eventData, GameObject clicked);
    protected virtual void Up(Transform holding)
    {
        if (clickType != ClickType.None)
        {
            holding.transform.localScale = Vector3.one;
        }      
    }
    protected virtual void Down(Transform holding)
    {
        if (clickType != ClickType.None)
        {
            holding.transform.localScale = Vector3.one * PopScale;
        }       
    }


    ITooltip _tooltip;
    public event Action<PointerEventData> OnTouched;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLock) return;
        GameObject clicked = eventData.pointerCurrentRaycast.gameObject;
        OnTouched?.Invoke(eventData);

        if (clicked.CompareTag("EventHolder"))
        {
            if (holding != null)
            {
                Up(holding.transform);
                holding = null;
            }
        }
        else if (clicked != holding)
        {
            if (holding != null) Up(holding.transform);
            holding = clicked;
        }
        else
        {
            Up(holding.transform);
            holding = null;
            if (ignoreDragProctector || !wasDragging)
            {
                if (ClickAction(eventData, clicked))
                {
                    if (clicked.TryGetComponent(out _tooltip))
                    {
                        _tooltip.Show();
                    }
                }
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLock) return;
        GameObject downed = eventData.pointerCurrentRaycast.gameObject;
        //Debug.Log("Down: " + downed.tag);
        if (downed.tag != "EventHolder")
        {
            holding = downed;
            Down(holding.transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLock) return;
        if (holding != null)
        {
            Up(holding.transform);
        }
        holding = null;
    }
}

public enum ClickType
{
    PopDown,
    PopUp,
    None,
}

public static class UIItemGetter
{
    public static T GetItem<T>(this GameObject obj) where T: IUIInteractable
    {
        return obj.GetComponentInParent<T>();
    }
}
