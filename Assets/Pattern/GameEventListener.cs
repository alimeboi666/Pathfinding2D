using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventDrivenAPI
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {
        public GameEvent gameEvent;
        public UnityEvent<object> response;

        private void OnEnable()
        {
            if (gameEvent != null)
                gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent != null)
                gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(object value)
        {
            if (response != null)
                response.Invoke(value);
        }
    }
}

