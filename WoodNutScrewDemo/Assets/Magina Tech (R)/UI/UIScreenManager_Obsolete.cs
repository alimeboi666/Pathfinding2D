using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Hung.Core;

public class UIScreenManager_Obsolete : Singleton<UIScreenManager_Obsolete>
{
    [SerializeField] private List<UIScreen_Obsolete> screens;
    [SerializeField] private List<UIScreen_Obsolete> travelScreens;
    [SerializeField] private List<UIScreen_Obsolete> screenStackShow;
    private Stack<UIScreen_Obsolete> screenStack = new Stack<UIScreen_Obsolete>();

    public bool isOnMenu
    {
        get => GetCurrent() == screens[0];
    }

    private void Awake()
    {
        //Test.T(() => PopupEvent.Instance.OnOpenClick(HomeScreen.RateMenu));
    }

    private void Start()
    {
        foreach (UIScreen_Obsolete screen in screens)
        {
            screen.OnGameStart();
        }
        foreach (UIScreen_Obsolete screen in travelScreens)
        {
            screen.OnGameStart();
        }
        screens[0].gameObject.SetActive(true);
        Add(screens[0]);

        StartOpen();        
    }

    private void StartOpen()
    {
        if (autoOpenQueue.Count > 0)
        {
            UIScreen_Obsolete screen = autoOpenQueue.Dequeue();

            if (screen.IsPopUp)
            {
                PopupEvent.OnOpenClick(screen);
                PopupEvent.OnNextCloseCommit(StartOpen);
            }
        }       
    }

    private Queue<UIScreen_Obsolete> autoOpenQueue = new Queue<UIScreen_Obsolete>();
    
    internal void AssignStartOpenning(HomeScreen screen)
    {
        autoOpenQueue.Enqueue(GetScreen(screen));
    }

    public UIScreen_Obsolete GetScreen(HomeScreen screen)
    {
        return screens[(int)screen];
    }

    public UIScreen_Obsolete GetScreen(TravelScreen screen)
    {
        return travelScreens[(int)screen];
    }

    public void Add(UIScreen_Obsolete screen)
    {
        screenStackShow.Add(screen);
        screenStack.Push(screen);
    }

    public int screenDepth
    {
        get => screenStack.Count;
    }

    public void Remove()
    {
        screenStackShow.RemoveAt(screenStack.Count - 1);
        screenStack.Pop();
    }

    public UIScreen_Obsolete GetCurrent()
    {
        if (screenStack.Count > 0)
        {
            if (screenStack.Peek() != screenStackShow[screenStackShow.Count - 1])
            {
                Debug.Log("Something wrong!");
            }

            return screenStack.Peek();
        }
        else return screens[0];
    }

    public void GoHome()
    {
        while(screenDepth > 1)
        {
            GetCurrent().gameObject.SetActive(false);
            Remove();
        }
    }
}

public enum HomeScreen
{
    MainMenu,
    ShopSkinMenu,
    LeveMenu,
    Ingame,
    Wingame,
    Losegame,
    RatePopup,
    NoticePopup,
    UnlockSkin
}

public enum TravelScreen
{
    
}
