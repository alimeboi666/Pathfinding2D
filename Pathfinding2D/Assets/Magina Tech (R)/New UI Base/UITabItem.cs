using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UITabItem : UI_Item, IDriven
{
    [SerializeField] private TMP_Text textName;
    public int tabIndex => transform.GetSiblingIndex();

    IDriven _parentDriver;
    public void OnDriven()
    {
        _parentDriver = transform.parent?.GetComponent<IDriven>();
        
        if (_parentDriver != null)
        {
            _parentDriver.OnDriven();
        }
    }

    public override void OnPicked()
    {
        textName.color = Color.white;
    }

    public override void OnUnpicked()
    {
        textName.color = Color.black;
    }
}
