using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Hung.UI.Button.PointerEvents;
using Hung.UI.Button.Clickable;
using Sirenix.OdinInspector;
using Hung.Core;

namespace Hung.UI.Button
{
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class SimpleButton : MonoBehaviour, IClickable, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IToggleable
    {
        [SerializeField] private UnityEvent clickEvent;

        [SerializeField] protected bool allowHoldEvent;

        [ShowIf("allowHoldEvent"), Range(0.25f, 2), SerializeField] float holdThreshold;
        [field: ShowIf("allowHoldEvent"), Range(0, 1), SerializeField] public float holdInterval { get; private set; }

        [SerializeField] private Image clickPane;
        [field: SerializeField] public Image eventTarget { get; private set; }
        [SerializeField] private List<PointerDownEvent> pointerDownEvents;
        [SerializeField] private ClickableConfig clickable;
        [field: SerializeField] public bool hasSoundClick {get; private set;}
        [field: SerializeField] public bool hasAds { get; set; }

        [ShowIf("hasAds"), SerializeField] GameObject adSignal;

        [ShowIf("hasSoundClick"), SerializeField] private AudioClip soundClick;

        public event Action OnFirstClick;
        public event Action<bool> OnVisualChanged;

        Action<bool> _onClickableChange;
        public event Action<bool> OnClickableChanged
        {
            add
            {
                value(isClickable);
                _onClickableChange += value;
            }

            remove
            {
                _onClickableChange -= value;
            }
        }

        public event Action EarlyCommitAction;

        public UnityEvent ClickEvent { get => clickEvent; set => clickEvent = value; }

        public Action CommitAction { get; set; }

        [field: ReadOnly, SerializeField] public bool isClickable { get; private set; }
        public bool isVisible => gameObject.activeSelf;

        WaitForSeconds threshold;
        WaitForSeconds eachHold;
        Coroutine holdingCoroutine;
        private void Awake()
        {
            threshold = new WaitForSeconds(holdThreshold);
            eachHold = new WaitForSeconds(holdInterval);

            if (hasAds)
            {
                Archetype.AdsIntegration.OnAdsRewardShowChanged += (show) =>
                {
                    adSignal.SetActive(show);
                };
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (hasAds && Archetype.AdsIntegration.canShowAd)
            {
                Archetype.AdsIntegration.TryToShowRewardedAd(() =>
                {
                    if (OnFirstClick != null)
                    {
                        OnFirstClick();
                        OnFirstClick = null;
                    }
                    if (hasSoundClick) Archetype.MasterSound.PlayUISound(soundClick);
                    EarlyCommitAction?.Invoke();
                    clickEvent.Invoke();
                    CommitAction?.Invoke();
                });
            }
            else
            {
                if (OnFirstClick != null)
                {
                    OnFirstClick();
                    OnFirstClick = null;
                }
                if (hasSoundClick) Archetype.MasterSound.PlayUISound(soundClick);
                EarlyCommitAction?.Invoke();
                clickEvent.Invoke();
                CommitAction?.Invoke();
            }                
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            foreach (PointerDownEvent downEvent in pointerDownEvents)
            {
                downEvent.Cast(this, eventData);
            }

            if (allowHoldEvent && !hasAds) holdingCoroutine = StartCoroutine(Holding());
        }

        IEnumerator Holding()
        {
            yield return threshold;
            while (true)
            {
                if (hasSoundClick) Archetype.MasterSound.PlayUISound(soundClick);
                clickEvent.Invoke();
                yield return eachHold;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            //Debug.Log("Button Up"); 
            foreach (PointerDownEvent downEvent in pointerDownEvents)
            {
                downEvent.Recast(this, eventData);
            }
            if (holdingCoroutine != null) StopCoroutine(holdingCoroutine);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("Button Exit");
            if (holdingCoroutine != null) StopCoroutine(holdingCoroutine);
        }

        public void ToggleOn()
        {
            gameObject.SetActive(true);
        }

        public void ToggleOff()
        {
            gameObject.SetActive(false);
        }

        public void SetClickable(bool set)
        {
            //if (gameObject.tag == "Testing") Debug.Log("Clickeable: " + set);
            isClickable = set;
            _onClickableChange?.Invoke(set);
            if (clickable != null) clickable.SetClickable(clickPane, eventTarget, set);
            //Debug.Log("Clickeable: " + set);
        }
        public void SetDormant(bool set)
        {
            clickPane.raycastTarget = !set;
        }

        public void ChangePaneColor(Color color)
        {
            eventTarget.color = color;
        }

        bool _firstTimeChangeSound = true;
        AudioClip originalSoundClick;
        public void TemporaryChangeSoundClick(AudioClip sound)
        {
            if (_firstTimeChangeSound)
            {
                _firstTimeChangeSound = false;
                originalSoundClick = soundClick;
            }
            soundClick = sound;
        }

        public void ChangeSoundClickToNormal()
        {
            if (!_firstTimeChangeSound) soundClick = originalSoundClick;
        }

    }
}

