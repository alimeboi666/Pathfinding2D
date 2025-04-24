using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI.Button.Clickable
{
    [CreateAssetMenu(menuName = "Hung/Button/Clickable Config/Sprite Change")]
    public class SpriteChange : ClickableConfig
    {
        [SerializeField] private Sprite clickable;
        [SerializeField] private Sprite unclickable;
        public override void SetClickable(Image clickPane, Image targetPane, bool set)
        {
            targetPane.sprite = set ? clickable : unclickable;
        }
    }

}
