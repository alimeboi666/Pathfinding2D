using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Hung/SO Singleton/Panel Event")]
public class PanelEvent : SOSingleton<PanelEvent>
{
    [SerializeField] internal float wipeDuration;
    [SerializeField] internal float popDuration;

    public void OnOpenClick(HomeScreen screenName)
    {
        Screen_On(UIScreenManager_Obsolete.Instance.GetScreen(screenName));
    }

    public void OnOpenClick(TravelScreen screenName)
    {
        Screen_On(UIScreenManager_Obsolete.Instance.GetScreen(screenName));
    }

    public void OnOpenClick(UIScreen_Obsolete screen)
    {
        Screen_On(screen);
    }

    public void OnExitClick(UIScreen_Obsolete screen)
    {
        Screen_Off(screen);
    }

    private void Screen_On(UIScreen_Obsolete screen)
    {
        UIScreenManager_Obsolete.Instance.Add(screen);
        screen.OnStartOpening();

        for (int i = 0; i < screen.WipedList.Count; i++)
        {
            screen.WipedList[i].DOAnchorPos(Vector2.zero, wipeDuration).SetUpdate(true).SetEase(Ease.OutBack);
        }
        float stepTime = screen.PopStepTime;
        int n = screen.PoppedList.Count;
        for (int i = 0; i < n; i++)
        {
            int index = i;
            screen.PoppedList[i].DOScale(1, popDuration).SetUpdate(true).SetEase(Ease.OutBack).SetDelay(wipeDuration + i * stepTime).OnComplete(() =>
            {
                if (index == n - 1)
                {
                    screen.OnOpened();
                }
            });
        }
    }

    private void Screen_Off(UIScreen_Obsolete screen)
    {
        UIScreenManager_Obsolete.Instance.Remove();

        screen.MaskBack();
        List<RectTransform> wipedList = screen.WipedList;

        int i;
        for (i = 0; i < wipedList.Count - 1; i++)
        {
            wipedList[i].DOAnchorPos(screen.WipedPosList[i], wipeDuration).SetUpdate(true).SetEase(Ease.InBack);
        }
        if (wipedList.Count > 0)
        {
            wipedList[i].DOAnchorPos(screen.WipedPosList[i], wipeDuration).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
            {
                screen.OnClosed();
            });
        }
        else
        {
            screen.OnClosed();
        }

        for (i = 0; i < screen.PoppedList.Count; i++)
        {
            screen.PoppedList[i].localScale = Vector3.zero;
        }
    }
}
