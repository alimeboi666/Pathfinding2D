//using Hieu.GameFlow;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class TypeFinder
{
    public static IEnumerable<T> FindMultiComponents<T>(this T caller, bool isIncludeInactive = false, bool ignoreSelf = false) where T: IMono
    {
        if (!ignoreSelf) return Object.FindObjectsOfType<MonoBehaviour>(isIncludeInactive).OfType<T>();
        else return Object.FindObjectsOfType<MonoBehaviour>(isIncludeInactive).OfType<T>().Where(i => i.gameObject != caller.gameObject);
    }


    public static List<T> FindMultiComponents<T>(bool isIncludeInactive = false)
    {
        return Object.FindObjectsOfType<MonoBehaviour>(isIncludeInactive).OfType<T>().ToList();
    }

    public static List<GameObject> FindGameObjectsOfComponent<T>() where T: IMono
    {
        IEnumerable<T> inters = Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
        HashSet<GameObject> set = new HashSet<GameObject>();

        foreach (T inter in inters)
        {
            set.Add(inter.gameObject);
        }

        return set.ToList();
    }
}
