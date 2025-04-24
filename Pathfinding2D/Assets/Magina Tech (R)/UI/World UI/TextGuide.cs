using System;
using System.Collections;
using System.Collections.Generic;
using Hung.Pooling;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextGuide : ObjectFollower, ICommonPoolable, IClickable, IDisposable, IPointerClickHandler
{
    [SerializeField] Image pane;
    [SerializeField] TMP_Text txt;

    Action _onFirstClicked;
    Action _onClick;

    public event Action OnFirstClick
    {
        add
        {
            pane.raycastTarget = true;
            _onFirstClicked += value;
        }

        remove
        {
            _onFirstClicked -= value;
            if (_onFirstClicked == null) pane.raycastTarget = false;
        }
    }

    public void SetClick(Action clickAction, bool isDisappearAfterClick = false)
    {
        pane.raycastTarget = true;
        _onClick = clickAction;
        if (isDisappearAfterClick) _onClick += () => Pool.BackToPool(this);
    }

    public void SetText(string content, float size = 27, float alpha = 1)
    {
        txt.text = content;
        txt.fontSize = size;
        var color = pane.color;
        color.a = alpha;
        pane.color = color;
    }

    [Button]
    public void SetAnchor(Vector2 offset)
    {
        pane.rectTransform.anchoredPosition = offset;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_onFirstClicked != null)
        {
            _onFirstClicked?.Invoke();
            _onFirstClicked = null;
        }
        _onClick?.Invoke();
    }

    private new void OnDisable()
    {
        base.OnDisable();
    }

    public void Dispose()
    {
        pane.raycastTarget = false;
        _onFirstClicked = null;
    }
}
