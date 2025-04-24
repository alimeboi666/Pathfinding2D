using DG.Tweening;
using Hung.Core;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityExtensions.Tween;

namespace Hung.UI
{
    public class UIScreen : SerializedMonoBehaviour, IToggleable, IVisualize, IDriven
    {
        //[SerializeField] protected TweenPlayer m_tweenPlayer;
        //[SerializeField] private GameEvent openEventCommand;

        [field: SerializeField] public bool isInit { get; private set; }

        [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

        [field: SerializeField] public bool isFullScreen { get; private set; }

        [field: SerializeField] public bool isPopUp { get; private set; }

        [field: SerializeField] public bool openSound { get; private set; }

        [field: SerializeField] public bool closeSound { get; private set; }

        private bool IsFullScreenOrPopUp() => isFullScreen || isPopUp;

        [field: SerializeField, ShowIf(nameof(isPopUp))] public Transform popUpTransform;
        [field: SerializeField, ShowIf(nameof(IsFullScreenOrPopUp))] public List<RectTransform> wipedList;
        [field: SerializeField, ShowIf(nameof(IsFullScreenOrPopUp))] public List<Vector2> wipedPopList;
        [field: SerializeField, ShowIf(nameof(IsFullScreenOrPopUp))] public List<RectTransform> poppedList;

        [field: SerializeField, ShowIf(nameof(IsFullScreenOrPopUp))] public float wipeDuration = 0.2f;
        [field: SerializeField, ShowIf(nameof(IsFullScreenOrPopUp))] public float popDuration = 0.2f;
        private float popStepTime = 0.25f;

        

        public event Action OnClosed;
        public event Action<bool> OnVisualChanged;

        public void OnDriven()
        {
            //m_tweener = GetComponent<UITweener>();
            //m_tweenPlayer = GetComponent<TweenPlayer>();
        }

        public virtual void OnPrepareStart()
        {
            //openEventCommand?.AddListener(() => VisualOn());
            if (!isInit) ToggleOff();
            else ToggleOn();         
        }

        public virtual void OnPrepareToStart()
        {
            if (isFullScreen && !isInit) ScreenOff();
        }

        //protected override void Awake()
        //{
        //    m_forceStarts.ForEach(starter => starter.GetComponent<IForceStart>().OnStart());
        //}

        public virtual void ToggleOn()
        {
            isVisible = true;
            gameObject.SetActive(true);
            OnVisualChanged?.Invoke(true);
         
        }

        public virtual void ToggleOff()
        {
            isVisible = false;
            OnVisualChanged?.Invoke(false);

            gameObject.SetActive(false);
            OnClosed?.Invoke();
            //OnDispose = null;
        }

        public virtual void VisualOn()
        {
            if (isFullScreen) ScreenOn();
            else if (isPopUp) PopUpOn();
            else ToggleOn();
            //Debug.Log("Screen " + name + " is visual on");
            if (openSound) Archetype.MasterSound.PlayUISound(UISound.ScreenOpen);
            //ToggleOn();
            //m_tweenPlayer?.Play();
        }

        public virtual void VisualOff()
        {        
            if (isFullScreen) ScreenOff();
            else if (isPopUp) PopUpOff();
            else ToggleOff();
            //Debug.Log("Screen " + name + " is visual off");
            if (closeSound) Archetype.MasterSound.PlayUISound(UISound.ScreenClose);
            //ToggleOff();
            //callback?.Invoke();
            //m_tweenPlayer?.Stop();
        }


        public virtual void ScreenOn()
        {
            Debug.Log("Screen On");
            ToggleOn();
            for (int i = 0; i < wipedList.Count; i++)
            {
                wipedList[i].DOAnchorPos(Vector2.zero, wipeDuration).SetUpdate(true).SetEase(Ease.OutExpo);
            }
            for (int i = 0; i < poppedList.Count; i++)
            {
                poppedList[i].DOScale(1, popDuration).SetUpdate(true).SetEase(Ease.OutBack).SetDelay(wipeDuration + i * popStepTime);
            }
        }

        public virtual void ScreenOff()
        {
            Debug.Log("Screen Off");
            List<RectTransform> wipedListCopy = wipedList;

            int i;
            for(i = 0; i < wipedListCopy.Count - 1; i++)
            {
                wipedList[i].DOAnchorPos(wipedPopList[i], wipeDuration).SetUpdate(true).SetEase(Ease.InQuad);
            }
            if (wipedList.Count > 0)
            {
                wipedList[i].DOAnchorPos(wipedPopList[i], wipeDuration).SetUpdate(true).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    ToggleOff();
                });

            }
            else
            {
                ToggleOff();
            }
            for (i = 0; i < poppedList.Count; i++)
            {
                Transform t = poppedList[i];
                t.DOKill(true);
                t.DOScale(0, popDuration).SetUpdate(true).SetEase(Ease.OutBack);      
            }
        }

        public virtual void PopUpOff()
        {
            Debug.Log("Popup Off");
            for(int i = 0;i < poppedList.Count; i++)
            {
                poppedList[i].localScale = Vector3.zero;
            }

            popUpTransform.DOScale(Vector3.zero, popDuration).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
            {
                ToggleOff();
            });
        }

        public virtual void PopUpOn()
        {
            Debug.Log("Popup On");
            popUpTransform.localScale = Vector3.one;
            ToggleOn();
            popUpTransform.localScale = Vector3.zero;

            popUpTransform.DOScale(Vector3.one, popDuration).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
            {
                float stepTime = popStepTime;
                for (int i = 0; i < poppedList.Count; i++)
                {
                    poppedList[i].DOScale(1, popDuration).SetUpdate(true).SetEase(Ease.OutBack).SetDelay(wipeDuration * i * stepTime);
                }
            });
        }
    }

}

