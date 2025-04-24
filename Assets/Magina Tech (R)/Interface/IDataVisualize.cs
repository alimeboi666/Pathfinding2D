using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Hung.GameData
{
    public interface IDataVisualize<T> : IDataHolder<T> where T : IIdentifiedData
    {
        void Set(T info);
    }

    public interface IDataUnitVisualize<StatHolder, T>: IDataHolder<T> where T: IIdentifiedData where StatHolder: IDataHolder<T>, IStat
    {
        StatHolder visualizeTarget { get; }

        void SetVisual(StatHolder dataHolder);
    }

    public interface IDataHolder<T> where T : IIdentifiedData
    {
        T info { get; }
    }

    public static class UIItemUtilities
    {
        public static int Match<T, U>(IEnumerable<T> data, List<U> slots, Action<T, U> onEach = null) where U : Component
        {
            var required = data.Count();
            var current = slots.Count;
            if (current >= required)
            {
                for (int i = required; i < current; i++)
                {
                    slots[i].gameObject.SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < required - current; i++)
                {
                    var newSlot = GameObject.Instantiate(slots[0], slots[0].transform.parent);
                    slots.Add(newSlot);
                }
            }

            for (int i = 0; i < required; i++)
            {
                slots[i].gameObject.SetActive(true);
                onEach?.Invoke(data.ElementAt(i), slots[i]);
            }
            return required;
        }
    }
}