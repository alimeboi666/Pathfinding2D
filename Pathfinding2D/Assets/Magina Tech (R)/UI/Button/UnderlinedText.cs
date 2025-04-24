using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

namespace Hung.UI.Button.PointerEvents
{
    [CreateAssetMenu(menuName = "Hung/Button/PointerDownEvent/Underlined Text")]
    public class UnderlinedText : PointerDownEvent
    {
        public override void Cast(SimpleButton caller, PointerEventData eventData)
        {
            caller.GetComponentInChildren<TMP_Text>().fontStyle = TMPro.FontStyles.Underline;
        }

        public override void Recast(SimpleButton caller, PointerEventData eventData)
        {
            caller.GetComponentInChildren<TMP_Text>().fontStyle = TMPro.FontStyles.Normal;
        }
    }
    
}