using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Sirenix.OdinInspector;
using Hung.Pooling;
using Hung.Core;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
namespace Hung.UI.Extensions
{
    public class UIScreenManager : MonoBehaviour, IUICanvas, ICache /*, Hieu.GameFlow.IGameplayPrepareStart*/
    {
        //[field:SerializeField] public Hieu.GameFlow.ExecuteLayer ExecuteLayer { get; private set; }

        [field:SerializeField] public Canvas targetCanvas { get; private set; }

        [field:SerializeField] public bool isEmpty { get; private set; }

        public event Action<bool> OnScreenEmptyChange;

        public bool isInteractable
        {
            get => raycaster.enabled;

            set => raycaster.enabled = value;
        }

        [SerializeField] private List<UIScreen> m_screens;
        [AssetsOnly, SerializeField] private UI_ScreenMask maskPrefab;
        [SerializeField] private GraphicRaycaster raycaster;

        public void OnCached()
        {
            //m_screens = TypeFinder.FindMultiComponents<UIScreen>(isIncludeInactive: true);
            m_screens = transform.GetComponentsInChildren<UIScreen>(includeInactive: true).ToList();
        }

        private void Awake()
        {
            targetCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        }

        public void OnGameplayPrepareStart()
        {
            m_screens.ForEach(screen =>
            {
                screen.OnPrepareToStart();
                if (!screen.isInit)
                {
                    var screenIndex = m_screens.IndexOf(screen);
                    screen.OnVisualChanged += (bool set) => SetBitAndCheck(set, screenIndex);
                }
            });
            isEmpty = true;
            OnScreenEmptyChange?.Invoke(true);
        }
        int m_screen_bits = 0x00;
        void SetBitAndCheck(bool set, int index)
        {
            int mask = 1 << index;
            if (set)
            {
                isEmpty = false;
                OnScreenEmptyChange?.Invoke(false);
                m_screen_bits |= mask;
            }
            else
            {
                m_screen_bits &= ~mask;
                //Debug.Log("Screen close: " + m_screen_bits);
                isEmpty = m_screen_bits == 0;
                OnScreenEmptyChange?.Invoke(isEmpty);
            }
        }

        //void Update()
        //{
        //    //isEmpty = m_screen_bits == 0;
        //    Debug.Log(Convert.ToString(m_screen_bits, 2).PadLeft(32, '0'));
        //}

        Dictionary<Type, UIScreen> screenDict = new();


        public T GetScreen<T>() where T : UIScreen
        {
            var screenType = typeof(T);

            if (screenDict.ContainsKey(screenType)) return screenDict[screenType] as T;
            else
            {
                var target = m_screens.Where(screen => screen is T).First();
                screenDict[screenType] = target;
                return target as T;
            }
        }

        public void SetMask(UIScreen screen, ScreenMaskType screenMaskType, float opacity = 0.6862769f, bool blockable = true)
        {
            var mask = Pool.Spawn(maskPrefab);
            mask.Set(screen, screenMaskType, opacity, blockable);

            void MaskBack()
            {
                Pool.BackToPool(mask);
                screen.OnClosed -= MaskBack;
            }

            screen.OnClosed += MaskBack;
        }

        public void ShowHelper(string content,string title = "Helper", Action gotoAction = null, Action dismissAction = null)
        {
            var screen = GetScreen<UIScreen_Helper>();
            screen.VisualOn();
            screen.Set(content, title, gotoAction, dismissAction);
        }

        public void SetOrderInLayer(int order, string layerName = "")
        {
            if (!string.IsNullOrEmpty(layerName) && SortingLayer.NameToID(layerName) != 0)
            {
                targetCanvas.sortingLayerName = layerName;
            }

            targetCanvas.sortingOrder = order;
        }
    }

}