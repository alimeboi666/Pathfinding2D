using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HashSetSimulator
{
    public static void VirtualAdd<T>(this ICollection<T> collection, T adding)
    {
        if (!collection.Contains(adding)) collection.Add(adding);
    }

    public static void VirtualClear<T>(this ICollection<T> collection)
    {
        collection.Clear();
    }
}