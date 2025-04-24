using System.Collections;
using System.Collections.Generic;
//using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI.Button.Clickable
{
    [CreateAssetMenu(menuName = "Hung/Button/Clickable Config/Grayscaler")]
    public class Grayscaler : ClickableConfig
    {
        public override void SetClickable(Image clickPane, Image targetPane, bool set)
        {
            //var effect = targetPane.GetComponent<UIEffect>();
            //if (effect == null) effect = targetPane.gameObject.AddComponent<UIEffect>();

            //effect.effectMode = set ? EffectMode.Grayscale : EffectMode.None;
        }
    }
}
