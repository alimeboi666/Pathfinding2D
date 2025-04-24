using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.StateMachine
{
    public class SM_Unit_Movement_Move : SM_Base<MovementState>
    {


        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        protected override void OnStateEntrance(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            State.StartMoving();
            
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (HasAnyTrigger())
            {
                //Debug.Log("LUL");
                animator.SetBool("isMoving", false);
            }
        }

        

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }

}

