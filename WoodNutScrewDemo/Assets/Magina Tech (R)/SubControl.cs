using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SubControl<T> : SerializedMonoBehaviour where T: MonoBehaviour
{
    protected T p_controller;

    protected T Arch
    {
        get
        {
            if (!p_controller)
            {
                p_controller = GetComponentInParent<T>();
            }
            return p_controller;
        }       
    }
}

public abstract class SubFlow<T>: MonoBehaviour where T: IFlow
{
    protected T p_controller;

    protected T Arch
    {
        get
        {
            if (p_controller == null)
            {
                p_controller = transform.parent.GetComponentInParent<T>();
            }
            return p_controller;
        }
    }
}
