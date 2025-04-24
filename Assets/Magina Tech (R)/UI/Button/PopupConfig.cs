using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hung/UI/Popup Config")]
public class PopupConfig : ScriptableObject
{
    [Range(0f, 1f)][SerializeField] internal float popUpDuration;
    [Range(0f, 1f)][SerializeField] internal float fadeDuration;
    [Range(0f, 1f)][SerializeField] internal float fadeValue;
    [SerializeField] internal float wipeDuration;
    [SerializeField] internal float popDuration;

}
