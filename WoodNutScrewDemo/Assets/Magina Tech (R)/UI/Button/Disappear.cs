using UnityEngine;
using UnityEngine.EventSystems;

namespace Hung.UI.Button.PointerEvents
{
    [CreateAssetMenu(menuName = "Hung/Button/PointerDownEvent/Disappear")]
    public class Disappear : PointerDownEvent
    {
        [SerializeField] private float scale;
        public override void Cast(SimpleButton caller, PointerEventData eventData)
        {
            caller.eventTarget.transform.localScale *= scale;
        }

        public override void Recast(SimpleButton caller, PointerEventData eventData)
        {
            caller.gameObject.SetActive(false);
            caller.OnPointerClick(null);
        }
    }

}
