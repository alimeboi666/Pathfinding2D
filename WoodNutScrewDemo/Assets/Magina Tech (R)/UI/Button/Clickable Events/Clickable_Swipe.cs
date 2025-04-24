using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI.Button.Clickable
{
    [CreateAssetMenu(menuName = "Hung/Button/Clickable Config/Swipe")]


    public class Swipe: ClickableConfig
    {

        [Range(0.25f, 0.5f), SerializeField] private float swipeDuration;

        public override void SetClickable(Image clickPane, Image targetPane, bool setOn)
        {
            if (setOn)
            {
                clickPane.raycastTarget = false;
                targetPane.rectTransform.anchoredPosition = new Vector2(-150, 0);
                targetPane.rectTransform.DOAnchorPos(Vector2.zero, swipeDuration).SetEase(Ease.OutBack)
                    .OnComplete(() => clickPane.raycastTarget = true);
            }
            else
            {
                clickPane.raycastTarget = false;
                targetPane.rectTransform.anchoredPosition = Vector2.zero;
                targetPane.rectTransform.DOAnchorPos(new Vector2(-150, 0), swipeDuration).SetEase(Ease.InBack);
            }
        }
    }
}
