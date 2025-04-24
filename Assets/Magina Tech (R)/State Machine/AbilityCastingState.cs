using Hung.AbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Hung.StateMachine
{
    public class AbilityCastingState : StateControl, IAbilityCastFlow
    {
        [field: ReadOnly, SerializeField] public int castIndex { get; set; }

        [field:SerializeField] public UnityEvent<int> castRelease { get; private set; }

        public void CastRelease(int castIndex)
        {
            castRelease.Invoke(castIndex);
        }

        public void CastReleaseAtCurrentIndex()
        {
            CastRelease(castIndex);
        }
    }

}

