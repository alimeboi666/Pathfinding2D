using Hung.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hung.StateMachine
{
    public class UnitState : StateControl, IUnitFlow
    {
        [field: SerializeField] public UnityEvent<IUnit> startDead { get; private set; }
        [field: SerializeField] public UnityEvent<IUnit> finishDead { get; private set; }

        public void StartDead()
        {
            startDead.Invoke(null);
        }

        public void FinishDead()
        {
            finishDead.Invoke(null);
        }

        public void Hit()
        {
            throw new System.NotImplementedException();
        }
    }
}