using Hung.Core;
using Hung.Testing;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UI_ListEventHolder<Item, Info> : UI_EventHolder, IForceStart, ITest where Item : UI_ListItem<Info> where Info: IIdentifiedData
{
    protected abstract string GameDataPath { get; }

    [SerializeField] protected Transform holder;

    [SerializeField] protected Item itemOrigin;

    [ReadOnly] [SerializeField] protected Item _selecting;

    [SerializeField] protected ItemInfoList<Info> infoList = new();

    protected delegate OwnedState OwnedChecker(int i);

    internal Info GetInfoById(int id) => infoList.list[id];
   
    internal Item GetItemById(int id) => holder.GetChild(id).GetComponent<Item>();

    internal int ItemLength => infoList.list.Length;

    protected abstract void InitializeInfoList(ref ItemInfoList<Info> infoList);

    public void OnStart()
    {
        Load(0, i => OwnedState.Available);
    }

    public void OnTest()
    {
        OnStart();
    }

    protected Item Load(int currentSelect, OwnedChecker ownedChecker)
    {
        //infoList = JsonExtension.Load<ItemInfoList<Info>>(GameDataPath, "Game Data/");

        InitializeInfoList(ref infoList);

        Item item = Hollow.Instance.Get<Item>();
        int skinLength = infoList.list.Length;
        int childLength = holder.childCount;
        for (int i = 0; i < skinLength; i++)
        {
            if (i < childLength)
            {
                item = holder.GetChild(i).GetComponent<Item>();
            }
            else
            {
                item = Instantiate(itemOrigin, holder);
            }
            item.Set(infoList.list[i]);//, ownedChecker(i));
            if (i == currentSelect)
            {
                _selecting = item;
            }
        }
      
        Transform leftOver;
        while (holder.childCount > skinLength)
        {
            //Debug.Log("Kekw");
            leftOver = holder.GetChild(skinLength);
            leftOver.parent = null;
            Destroy(leftOver.gameObject);
        }

        return _selecting;
    }

    protected override bool ClickAction(PointerEventData eventData, GameObject clicked)
    {
        return OnClickAnItem(eventData, clicked, clicked.GetItem<Item>());
    }

    protected abstract bool OnClickAnItem(PointerEventData eventData, GameObject clicked, Item itemOfClicked);
}
