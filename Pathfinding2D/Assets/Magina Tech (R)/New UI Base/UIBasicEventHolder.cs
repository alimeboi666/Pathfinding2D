using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBasicEventHolder : UI_EventHolder
{
    UI_Item _picked;
    protected override bool ClickAction(PointerEventData eventData, GameObject clicked)
    {
        if (clicked.TryGetComponent(out UI_Item item))
        {
            if (_picked != item)
            {
                _picked?.OnUnpicked();
                _picked = item;
                _picked.OnPicked();

                return true;
            }
        }
        return false;
    }
}
