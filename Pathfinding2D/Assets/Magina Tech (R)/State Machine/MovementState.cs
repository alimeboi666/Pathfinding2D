using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hung.StateMachine
{
    public interface IMoveFlow: IFlow
    {
        void StartMoving();

        void StopMoving();
    }

    public class MovementState : StateControl, IMoveFlow
    {
        [SerializeField] private UnityEvent startMoving;

        [SerializeField] private UnityEvent stopMoving;

        public void StartMoving()
        {
            startMoving.Invoke();
        }

        public void StopMoving()
        {
            stopMoving.Invoke();
        }
    }
}