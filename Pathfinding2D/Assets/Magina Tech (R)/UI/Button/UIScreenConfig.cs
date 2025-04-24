using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hung/UI/UI Screen Config")]
public class UIScreenConfig : ScriptableObject
{
    [SerializeField] internal float wipeDuration;
    [SerializeField] internal float popDuration;
}
