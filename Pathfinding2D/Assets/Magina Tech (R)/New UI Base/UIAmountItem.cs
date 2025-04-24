using Hung.GameData;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAmountItem : UI_Item, IDataHolder<GameData>
{
    [field: ReadOnly][field: SerializeField] public GameData info { get; private set; }

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text textContent;
  
    public void SetInfo(GameData item, int amount = 1, bool showNameInstead = false)
    {
        info = item;

        icon.sprite = item.Icon;
        textContent.text = !showNameInstead ? ("x" + amount) : item.Name;
    }

    public override void OnPicked()
    {
                        
    }

    public override void OnUnpicked()
    {
        
    }
}
