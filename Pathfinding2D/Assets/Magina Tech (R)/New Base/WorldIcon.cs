using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Hung.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldIcon : ObjectFollower, ICommonPoolable, IDisposable, IClickable, IPointerClickHandler, IPointerUpHandler
{
    [SerializeField] private Image icon;

    public void SetIcon(Sprite sprite, bool nativeSize = false)
    {
        icon.sprite = sprite;
        if (nativeSize) icon.SetNativeSize();
    }

    public event Action OnFirstClick;
    Action _onClick;
    [field: ReadOnly, SerializeField] public bool isClicked { get; private set; }

    public void SetClick(Action clickAction, bool isDisappearAfterClick = false)
    {
        icon.raycastTarget = true;
        _onClick = clickAction;
        if (isDisappearAfterClick) _onClick += () => Pool.BackToPool(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnFirstClick != null)
        {
            isClicked = true;
            OnFirstClick();
            OnFirstClick = null;
            //icon.raycastTarget = false;
        }
        _onClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        isClicked = false;       
    }


    public void Dispose()
    {
        icon.raycastTarget = false;
        _onClick = null;
    }

    public void DisableEffect()
    {
        //GetComponent<CommonTweener>().enabled = false;
    }

    public void ScaleIcon()
    {
        this.gameObject.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

}
