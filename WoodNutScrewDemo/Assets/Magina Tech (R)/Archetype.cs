using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ScriptableObjectArchitecture;
using System;
using Hung.UI;
using UnityEngine.UI;

namespace Hung.Core
{
    public interface IScreenListener : ISingletonRole
    {
        event Action<Vector2> OnEmptyClick;

        event Action<Vector2> OnDragging;

        bool isHolding { get; }

        bool isLock { get; set; }
    }

    public interface IMasterInput : ISingletonRole
    {
        Vector2 GetAxis();

        Vector3 Get3DAxis();

        bool GetTestKey(int index);

        void AddAxisListener(Action<Vector2> action);

    }

    public interface IMasterHaptic : ISingletonRole, ISettingOptionToggler<IPlayShake>
    {
        bool isHapticOn { get; set; }

        void Vibrate();
    }

    public enum UISound
    {
        ScreenOpen,
        ScreenClose,
        ButtonClick,
        ResourceClaim,
        WinStreak
    }
    public interface IMasterSound : ISingletonRole, ISettingOptionToggler<IPlaySound>
    {
        bool isMusicOn { get; set; }

        bool isSoundOn { get; set; }

        bool isVibrationOn { get; set; }

        event Action<bool> onSoundChanged;

        event Action<bool> onVibrationChanged;

        void PlayWorldSound(AudioClip sound, Vector3 position);

        void PlayUISound(AudioClip sound);

        void PlayUISound(AudioClip sound, bool isLoop);

        void PlayUISound(UISound sound);

        void PlayLevelSound(int level);

        void PlayBackground(string source, AudioClip sound);

        void StopBackground(string source);

        void ChangeBackground(string source, AudioClip sound);

        void ChangeAllBackgrounds(bool isMute);

        void PlayVibration();
    }

    public interface IPlayer : ISingletonRole, IStat
    {

    }

    public interface IMainCamera : ISingletonRole, ICamera
    {
        Vector3 direction { get; }

        bool isUnderControl { get; }

        event Action<float> OnCameraSizeChanged;

        void ResetRange();

        void ZoomAtScale(float scale = 1);

        void Normalize();

        void FocusOn(Vector3 position, Action onComplete = null, float lensPercent = -1);

        void FocusOut(Action onComplete = null);
    }

    public interface IUICanvas: ISingletonRole, ICanvas
    {
        event Action<bool> OnScreenEmptyChange;

        bool isEmpty { get; }

        bool isInteractable { get; set; }

        T GetScreen<T>() where T : UIScreen;

        void SetMask(UIScreen screen, ScreenMaskType screenMaskType, float opacity = 0.6862769f, bool blockable = true);

        /// <summary>
        /// Show a helper pop-up
        /// </summary>
        /// <param name="content"></param>
        /// <param name="gotoAction"></param>
        void ShowHelper(string content, string title = "Helper", Action gotoAction = null, Action dismissAction = null);

        void SetOrderInLayer(int order, string layerName = "");
    }

    public interface ILayer
    {
        public const int MAX_LAYER = 10;
        
        void AssignLayer(Transform ui, int layer);

        void RemoveLayer(Transform ui, int layer);
    }

    public interface IWorldCanvas: ISingletonRole, ICanvas
    {
        bool isInteractable { get; set; }
    }

    public interface ISceneLoader: ISingletonRole, ICanvas
    {
        void Hide();

        void ShowSceneLoading();

        void RunAwait(Action longAction);
    }

    public interface ITutorial: ISingletonRole, ICanvas
    {
        event Action TutorialConfiguration;

        UI_ScreenMask mask { get; }

        bool CheckPass(TutorialFlow flow);

        void SetMask(bool isOn);

        void SetBlockAll(bool isOn);

        bool GetMask();

        ObjectFollower ShowIndicatorAt(Transform transform);

        ObjectFollower ShowIndicatorToMove(Transform current, Transform target);

        void TravelRect(Transform transform);

        void TravelBack(Transform transform);

        void TravelToTarget(Transform current, Transform target);
    }

    public enum AdvertimentClickType
    {
        None,
        Rewarded,
        RewardedInterstitial
    }

    public interface IAdsIntegration: ISingletonRole
    {
        bool canShowAd { get; set; }

        float Positive { get; set; }

        float Negative { get; set; }

        int adIgnoreStack { get; set; }

        event Action<bool> OnAdsRewardShowChanged;

        event Action OnAdFocusIn;

        event Action OnAdFocusOut;

        void TryToShowRewardedAd(Action onSuccess = null, Action onFailed = null);

        void TryToShowRewardedInterstitialAd(Action onSuccess = null, Action onFailed = null);

        bool AttempToShowRewardedAd();

        bool AttemptToShowRewardedInterstitialAd();

        bool AttemptToShowInterstitialAd();

        void ShowInterstitialAd();
    }

    public interface IAutomation: ISingletonRole
    {
        void ToggleCommandLine();

        bool showFPS { get; set; }

        bool showDebugger { get; set; }

        void SetVersionInfo(string info);
    }

    public interface IArchChrono: ISingletonRole
    {
        float timeScale { get; set; }

        void Pause();

        void Resume();
    }

    public interface ISingletonRole : IMono
    {

    }

    public static class Archetype
    {
        static IPlayer _player;
        static IMainCamera _mainCamera;
        static IUICanvas _canvasScreen;
        static IGameplayFlow _gameFlow;
        //static Hieu.GameFlow.IGameplayFlow H_gameFlow;
        static IMasterSound _masterSound;
        static IMasterHaptic _masterHaptic;
        static IMasterInput _masterInput;
        static IScreenListener _screenListener;
        static IWorldCanvas _worldCanvas;
        static ISceneLoader _sceneLoader;
        static ITutorial _tutorial;
        static IAdsIntegration _adsIntegration;
        static IAutomation _automation;
        static IArchChrono _chrono;
        //static Hieu.Tutorial.ITutorial _tutorial2;

        public static void BlockInput(bool isBlock)
        {
            ScreenListener.isLock = isBlock;
            UIManager.isInteractable = !isBlock;
            WorldCanvas.isInteractable = !isBlock;
        }

        public static bool isAllFree => UIManager.isEmpty && !Cameraman.isUnderControl;

        private static void TryToFindSingleton<T>(ref T singleton) where T : ISingletonRole
        {
            //if (singleton != null) return;
            try
            {
                if (singleton == null || singleton.gameObject == null)
                {
                    singleton = TypeFinder.FindMultiComponents<T>().First();
                }
            }
            catch
            {
                try
                {
                    singleton = TypeFinder.FindMultiComponents<T>().First();
                }
                catch
                {
                    singleton = default(T);
                }
            }
        }

        public static IPlayer Player
        {
            get
            {
                TryToFindSingleton(ref _player);
                return _player;
            }
        }

        public static IMainCamera Cameraman
        {
            get
            {
                TryToFindSingleton(ref _mainCamera);
                return _mainCamera;
            }
        }

        public static IUICanvas UIManager
        {
            get
            {
                TryToFindSingleton(ref _canvasScreen);
                return _canvasScreen;
            }
        }

        //public static Hieu.GameFlow.IGameplayFlow H_GameFlow
        //{
        //    get
        //    {
        //        TryToFindSingleton(ref H_gameFlow);
        //        return H_gameFlow;
        //    }
        //}

        public static IGameplayFlow GameFlow
        {
            get
            {
                TryToFindSingleton(ref _gameFlow);
                return _gameFlow;
            }
        }

        public static IMasterSound MasterSound
        {
            get
            {
                TryToFindSingleton(ref _masterSound);
                return _masterSound;
            }
        }

        public static IMasterHaptic MasterHaptic
        {
            get
            {
                TryToFindSingleton(ref _masterHaptic);
                return _masterHaptic;
            }
        }

        public static IMasterInput MasterInput
        {
            get
            {
                TryToFindSingleton(ref _masterInput);
                return _masterInput;
            }
        }

        public static IScreenListener ScreenListener
        {
            get
            {
                TryToFindSingleton(ref _screenListener);
                return _screenListener;
            }
        }

        public static IWorldCanvas WorldCanvas
        {
            get
            {
                TryToFindSingleton(ref _worldCanvas);
                return _worldCanvas;
            }
        }

        public static ISceneLoader SceneLoader
        {
            get
            {
                TryToFindSingleton(ref _sceneLoader);
                return _sceneLoader;
            }
        }

        public static ITutorial Tutorial
        {
            get
            {
                TryToFindSingleton(ref _tutorial);
                return _tutorial;
            }
        }
        //public static Hieu.Tutorial.ITutorial Tutorial2
        //{
        //    get
        //    {
        //        TryToFindSingleton(ref _tutorial2);
        //        return _tutorial2;
        //    }
        //}


        public static IAdsIntegration AdsIntegration
        {
            get
            {
                TryToFindSingleton(ref _adsIntegration);
                return _adsIntegration;
            }
        }

        public static IAutomation Automation
        {
            get
            {
                TryToFindSingleton(ref _automation);
                return _automation;
            }
        }

        public static IArchChrono TimeControl
        {
            get
            {
                TryToFindSingleton(ref _chrono);
                return _chrono;
            }
        }
    }

}

