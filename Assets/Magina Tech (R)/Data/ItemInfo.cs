using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInfo : IIdentifiedData
{
    public int ID { get; private set; }

    public string Name {get; private set; }

    public string Description { get; private set; }
}

[Serializable]
public class ScoreInfo: ItemInfo
{
    public int score;
}

[Serializable]
public class SkinInfo: ItemInfo
{
    
}

[Serializable]
public class LevelInfo: ItemInfo
{

}

public class ItemInfoList<T> where T: IIdentifiedData
{
    public T[] list;
}

[Serializable]
public class SkinShopInfo: ItemInfoList<SkinInfo>
{

}

[Serializable]
public class LevelListInfo: ItemInfoList<LevelInfo>
{

}

[Serializable]
public class ScoreList: ItemInfoList<ScoreInfo>
{

}