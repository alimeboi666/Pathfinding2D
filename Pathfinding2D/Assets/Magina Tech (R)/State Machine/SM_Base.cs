using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.StateMachine
{
    public abstract class SM_Base<T> : StateMachineBehaviour where T : StateControl
    {

        bool _hasInit;
        Animator m_animator;
        protected T State;
        List<int> triggerHashList = new();

        protected void ResetAllTriggers()
        {
            foreach(var hash in triggerHashList)
            {
                m_animator.ResetTrigger(hash);
            }
        }

        public bool HasAnyTrigger()
        {
            if (m_animator == null) return false;
            foreach (var param in m_animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Trigger && m_animator.GetBool(param.nameHash))
                {
                    return true;
                }
            }
            return false;
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        sealed override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_hasInit)
            {
                _hasInit = true;
                m_animator = animator;
                State = animator.GetComponent<T>();

                foreach (var param in animator.parameters)
                {
                    if (param.type == AnimatorControllerParameterType.Trigger)
                    {
                        triggerHashList.Add(param.nameHash);
                    }
                }

            }
            OnStateEntrance(animator, stateInfo, layerIndex);
        }

        protected abstract void OnStateEntrance(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{

        //}

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