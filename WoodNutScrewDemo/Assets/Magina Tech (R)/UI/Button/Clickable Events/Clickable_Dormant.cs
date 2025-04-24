using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI.Button.Clickable
{
    [CreateAssetMenu(menuName = "Hung/Button/Clickable Config/Dormant")]

    public class Dormant: ClickableConfig
    {
        [SerializeField] private Color clickable;
        [SerializeField] private Color unclickable;
        public override void SetClickable(Image clickPane, Image targetPane, bool setOn)
        {
            //Debug.Log(clickPane);
            //Debug.Log("Clickable: " + setOn);
            clickPane.raycastTarget = setOn;
            targetPane.color = setOn ? clickable : unclickable;
        }
    }
}
