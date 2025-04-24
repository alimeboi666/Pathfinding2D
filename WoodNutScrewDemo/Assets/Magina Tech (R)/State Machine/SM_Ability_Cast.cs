using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.StateMachine
{
    public class SM_Ability_Cast : SM_Base<AbilityCastingState>
    {
        protected override void OnStateEntrance(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            State.castIndex = (int)animator.GetFloat("typeSpell");
        }
    }
}
