using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[CreateAssetMenu(menuName = "Hung/SO Singleton/Popup Event")]
public class PopupEvent : SOSingleton<PopupEvent>
{
    [Range(0f, 1f)][SerializeField] internal float popUpDuration;
    [Range(0f, 1f)][SerializeField] internal float fadeDuration;
    [Range(0f, 1f)][SerializeField] internal float fadeValue;
    [SerializeField] internal float wipeDuration;
    [SerializeField] internal float popDuration;

    public static void OnOpenClick(HomeScreen screenName)
    {
        PopUp_On(UIScreenManager_Obsolete.Instance.GetScreen(screenName));
    }

    public static void OnOpenClick(TravelScreen screenName)
    {
        PopUp_On(UIScreenManager_Obsolete.Instance.GetScreen(screenName));
    }

    public void OnExitClick(TravelScreen screenName)
    {
        PopUp_Off(UIScreenManager_Obsolete.Instance.GetScreen(screenName));
    }


    public static void OnOpenClick(UIScreen_Obsolete screen)
    {
        PopUp_On(screen);
    }

    public void OnExitClick(UIScreen_Obsolete screen)
    {
        PopUp_Off(screen);
    }

    public void InstantExit(UIScreen_Obsolete screen)
    {
        screen.BgMask.DOFade(0, fadeDuration).SetUpdate(true);
        screen.BgMask.transform.SetParent(screen.transform);
        screen.transform.localScale = Vector3.zero;
        screen.OnClosed();
    }

    private static void PopUp_On(UIScreen_Obsolete screen)
    {
        screen.OnStartOpening();

        Transform popUpTransform = screen.transform;
        popUpTransform.gameObject.SetActive(true);
        screen.BgMask.color = Color.black * Instance.fadeValue;
        screen.BgMask.transform.SetParent(screen.transform.parent);
        screen.BgMask.transform.localScale = Vector3.one;
        screen.BgMask.transform.SetSiblingIndex(screen.transform.GetSiblingIndex());

        popUpTransform.DOScale(Vector3.one, Instance.popUpDuration).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() =>
        {
            screen.OnOpened();
            for (int i = 0; i < screen.WipedList.Count; i++)
            {
                screen.WipedList[i].DOAnchorPos(Vector2.zero, Instance.wipeDuration).SetUpdate(true).SetEase(Ease.OutBack);
            }
            float stepTime = screen.PopStepTime;
            for (int i = 0; i < screen.PoppedList.Count; i++)
            {
                screen.PoppedList[i].DOScale(1, Instance.popDuration).SetUpdate(true).SetEase(Ease.OutBack).SetDelay(Instance.wipeDuration + i * stepTime);
            }
        });
    }

    private Action closedCommittee;

    public float PopDuration { get => popDuration; }

    internal static void OnNextCloseCommit(Action action)
    {
        Instance.closedCommittee += action;
    }

    private static void PopUp_Off(UIScreen_Obsolete screen)
    {
        Transform popUpTransform = screen.transform;
        screen.BgMask.DOFade(0, Instance.fadeDuration).SetUpdate(true);

        for (int i = 0; i < screen.PoppedList.Count; i++)
        {
            screen.PoppedList[i].localScale = Vector3.zero;
        }

        popUpTransform.DOScale(Vector3.zero, Instance.popUpDuration).SetEase(Ease.InBack).SetUpdate(true).OnComplete(() =>
        {
            screen.OnClosed();
            screen.BgMask.transform.SetParent(screen.transform);

            if (Instance.closedCommittee != null)
            {
                Instance.closedCommittee();
                Instance.closedCommittee = null;
            }
        });
    }
}
