using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UINavigator : UI_EventHolder<UITabItem>, IDriven
{
    [Range(0, 0.05f)][SerializeField] private float offset;

    [SerializeField] private IntGameEvent tabChangeEvent;

    Vector2 anchorMin = new Vector2(1, 0);
    Vector2 anchorMax = new Vector2(1, 1);

    public void OnDriven()
    {
        items = GetComponentsInChildren<UITabItem>();
        
        int n = items.Length;
        float delta = 1f / n;

        for (int i = 0; i < n; i++)
        {
            anchorMin.x = delta * i + (i != 0 ? offset: 0);
            items[i].rect.anchorMin = anchorMin;

            anchorMax.x = delta * (i + 1) - ((i != n - 1) ? offset : 0);
            items[i].rect.anchorMax = anchorMax;
        }
    }

    UITabItem _current;
    protected override void ClickItem(UITabItem item)
    {
        if (item == _current) return;
        else
        {
            _current?.OnUnpicked();
            _current = item;
            _current.OnPicked();
            tabChangeEvent.Raise(_current.tabIndex);
        }
    }
}
