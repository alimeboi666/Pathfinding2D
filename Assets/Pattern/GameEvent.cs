using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventDrivenAPI
{
    [CreateAssetMenu(menuName = "Event System/GameEvent")]
    public class GameEvent : ScriptableObject, IGameEvent
    {
        private readonly List<IGameEventListener> listeners = new List<IGameEventListener>();

        public void Raise(object value = null)
        {
            foreach (var listener in listeners)
            {
                listener.OnEventRaised(value);
            }
        }

        public void RegisterListener(IGameEventListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void UnregisterListener(IGameEventListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }
    }

    public interface IGameEventListener
    {
        void OnEventRaised(object value);
    }

    public interface IGameEvent
    {
        void Raise(object value = null);
        void RegisterListener(IGameEventListener listener);
        void UnregisterListener(IGameEventListener listener);
    }
}