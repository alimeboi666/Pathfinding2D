using UnityEngine;
using UnityEngine.EventSystems;

namespace Hung.UI.Button.PointerEvents
{
    [CreateAssetMenu(menuName = "Hung/Button/PointerDownEvent/PopDown")]
    public class Pop : PointerDownEvent
    {
        [SerializeField] private float scale;
        public override void Cast(SimpleButton caller, PointerEventData eventData)
        {
            caller.eventTarget.transform.localScale *= scale;
        }

        public override void Recast(SimpleButton caller, PointerEventData eventData)
        {
            caller.eventTarget.transform.localScale /= scale;
        }
    }

}