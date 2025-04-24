using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnumerableExtensions
{
    public static T RandomElement<T>(this IEnumerable<T> list)
    {
        if (list == null) return default(T);
        return list.ElementAt(Random.Range(0, list.Count()));
    }
}
