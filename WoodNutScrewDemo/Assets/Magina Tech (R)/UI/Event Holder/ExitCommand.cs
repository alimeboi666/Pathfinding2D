using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCommand : MonoBehaviour
{
    public void OnExit()
    {
        var visual = transform.parent.GetComponentInParent<IVisualize>();
        if (visual != null)
        {
            visual.VisualOff();
        }
        else
        {
            var toggle = transform.parent.GetComponentInParent<IToggleable>();
            if (toggle != null)
            {
                Debug.Log("Close on " + toggle.gameObject.name); 
                toggle.ToggleOff();
            }
        }
    }
}
