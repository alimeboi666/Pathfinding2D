using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.GameData.IAP
{
    [CreateAssetMenu(menuName = "Hung/Data/Package")]
    public class PackageData : GameData
    {
        [field: SerializeField] public GameObject Visual { get; private set; }

        [field: SerializeField] public List<GameItemAmount> Items { get; private set; }
    }

    [Serializable]
    public struct GameItemAmount
    {
        public GameData item;
        public int amount;
    }

}

