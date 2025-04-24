using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.StateMachine
{
    public class SM_Unit_Movement_Idle : SM_Base<MovementState>
    {
        protected override void OnStateEntrance(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            State.StopMoving();
            if (!HasAnyTrigger())
            {
                animator.SetBool("isMoving", true);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetAllTriggers();
        }
    }
}

