using System.Collections;
using System.Collections.Generic;
using Hung.Pooling;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hung.UI
{
    public enum ScreenMaskType
    {
        Behind,
        Front
    }

    public enum ScreenMaskOpacity
    {
        None = 0,
        Light = 64,
        Medium = 166,
        Bold = 230
    }

    public enum ScreenMaskClickType
    {
        None,
        ScreenExit
    }

    public class UI_ScreenMask : ExitCommand, IPoolable, IPointerClickHandler
    {
        private Image m_mask;

        [SerializeField] private ScreenMaskClickType m_type;

        public PoolPartition pooling { get; set; }
        public bool isNewPooling { get; set; }

        private void Awake()
        {
            m_mask = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_type == ScreenMaskClickType.None) return;
            OnExit();
        }

        public void Set(UIScreen screen, ScreenMaskType type, float opacity, bool blockable = true)
        {
            transform.SetParent(screen.transform);
            var rect = transform as RectTransform;

            rect.anchorMax = Vector2.one;
            rect.anchorMin = Vector2.zero;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            m_mask.raycastTarget = blockable;
            var color = m_mask.color;
            color.a = opacity;
            m_mask.color = color;

            switch (type)
            {
                case ScreenMaskType.Behind: transform.SetAsFirstSibling(); break;

                case ScreenMaskType.Front: transform.SetAsLastSibling(); break;
            }
        }

        public void SetOpacity(ScreenMaskOpacity opacity)
        {
            var color = m_mask.color;
            color.a = (int)opacity / 255f;
            m_mask.color = color;
        }

        public void ToggleOn()
        {
            gameObject.SetActive(true);
        }

        public void ToggleOff()
        {
            gameObject?.SetActive(false);
        }
    }

}