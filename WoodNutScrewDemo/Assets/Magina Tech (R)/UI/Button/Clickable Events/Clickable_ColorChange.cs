using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI.Button.Clickable
{
    [CreateAssetMenu(menuName = "Hung/Button/Clickable Config/Color Change")]
    public class Clickable_ColorChange : ClickableConfig
    {
        [SerializeField] private Color clickable;
        [SerializeField] private Color unclickable;
        public override void SetClickable(Image clickPane, Image targetPane, bool set)
        {
            targetPane.color = set ? clickable : unclickable;
        }
    }

}
