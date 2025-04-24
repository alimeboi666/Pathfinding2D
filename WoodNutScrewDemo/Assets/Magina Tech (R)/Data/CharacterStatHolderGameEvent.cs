using Hung.GameData.RPG;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hung.StatSystem.Binding
{
    [System.Serializable]
    [CreateAssetMenu(
        fileName = "CharacterStatHolderGameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "Character Stat Holder",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS + 1)]
    public sealed class CharacterStatHolderGameEvent : GameEventBase<IStatHolder<UnitBindingStat>>
    {

    }
}