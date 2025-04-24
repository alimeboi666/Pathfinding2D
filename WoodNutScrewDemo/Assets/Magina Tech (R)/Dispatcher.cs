using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    public static class Announcer
    {
        internal static void Announce<T>(this MonoBehaviour caster, T info) where T: IComparable
        {
            Dispatcher.Cast(info, caster);
        }
    }

    public class Dispatcher : MonoBehaviour
    {
        static Dictionary<MonoBehaviour, Action<IComparable>> receiverMap = new Dictionary<MonoBehaviour, Action<IComparable>>();
        static Dictionary<MonoBehaviour, Action> dispelMap = new Dictionary<MonoBehaviour, Action>();

        internal static void Register<T>(MonoBehaviour announcer, Action<T> onReceived, Action onDispeling = null) where T : IComparable
        {
            if (receiverMap.ContainsKey(announcer))
            {
                receiverMap[announcer] += ((IComparable info) => onReceived((T)info));
                //Debug.Log(receiverMap[announcer].GetInvocationList().Length); 
            }
            else
            {
                receiverMap[announcer] = (IComparable info) => onReceived((T)info);
            }
            if (onDispeling != null)
            {
                if (dispelMap.ContainsKey(announcer))
                {
                    dispelMap[announcer] += onDispeling;
                }
                else
                {
                    dispelMap[announcer] = onDispeling;
                }
            }
        }

        internal static void Dismiss<T>(MonoBehaviour announcer, Action<T> onReceived) where T : IComparable
        {
            if (receiverMap.ContainsKey(announcer))
            {
                //receiverMap[announcer] -= (IFormattable info) => onReceived((T)info);
                
            }
        }

        internal static void Cast<T>(T info, MonoBehaviour announcer) where T : IComparable
        {
            if (receiverMap.ContainsKey(announcer))
            {
                receiverMap[announcer](info);
            }            
        }

        internal static void Dispel(MonoBehaviour announcer)
        {
            dispelMap[announcer]();
        }
    }

}
