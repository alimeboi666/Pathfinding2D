using UnityEngine;
using UnityEngine.EventSystems;

namespace Hung.UI.Button.PointerEvents
{

    [CreateAssetMenu(menuName = "Hung/Button/PointerDownEvent/SpriteChange")]
    public class SpriteChange : PointerDownEvent
    {

        public override void Cast(SimpleButton caller, PointerEventData eventData)
        {
            caller.eventTarget.gameObject.SetActive(true);
        }

        public override void Recast(SimpleButton caller, PointerEventData eventData)
        {
            caller.eventTarget.gameObject.SetActive(false);
        }
    }

}
