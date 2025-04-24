using Hung.GameData;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hung.UI
{
    
}

public interface IUIInteractable: IPickable
{

}

public abstract class UI_Item : UIBehaviour, IUIInteractable
{
    RectTransform _rect;
    public RectTransform rect
    {
        get
        {
            if (_rect != null) return _rect;
            else
            {
                _rect = (RectTransform)transform;
                return _rect;
            }
        }
    }

    public abstract void OnPicked();

    public abstract void OnUnpicked();
}

public abstract class UI_StaticItem<T> : UIBehaviour, IDataVisualize<T> where T : IIdentifiedData
{
    [field: SerializeField] public T info { get; private set; }

    protected override void Awake()
    {
        Set(info);
    }

    public virtual void Set(T info)
    {
        this.info = info;
    }

}

public abstract class UI_ListItem<T> : UI_Item, IDataVisualize<T> where T : IIdentifiedData
{
    [field: ReadOnly, SerializeField] public T info { get; private set; }

    public virtual void Set(T info)
    {
        this.info = info;
        //if (this is INoti noti && noti.hasNotification)
        //{

        //}
    }
}

public abstract class UI_InfoPane<StatHolder, T>: UIBehaviour, IDataUnitVisualize<StatHolder, T> where StatHolder: IDataHolder<T>, IStat where T : IIdentifiedData
{
    [field: SerializeField] public StatHolder visualizeTarget { get; private set; }

    [field: ReadOnly, SerializeField] public T info { get; private set; }

    public virtual void SetVisual(StatHolder dataHolder)
    {
        visualizeTarget = dataHolder;
        info = visualizeTarget.info;
    }
}

public enum OwnedState
{
    Lock,
    Available,
    Owned
}