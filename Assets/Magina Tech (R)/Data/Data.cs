using Hung.GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIdentifiedData
{
    int ID { get; }

    string Name { get; }

    string Description { get; }
}

public interface IIAPPurchasable
{
    void OnIAPPurchased(int amount = 1);
}

public interface IEffectOrigin : IIdentifiedData
{
   
}