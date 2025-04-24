using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUltis
{

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (!component)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}