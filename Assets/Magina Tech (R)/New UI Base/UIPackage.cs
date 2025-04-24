using Hung.GameData.IAP;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIPackage : UI_StaticItem<PackageData>, ICache
{
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textDes;
    [SerializeField] private Transform visualHolder;

    [SerializeField] private List<UIAmountItem> purchaseItems;

    public void OnCached()
    {
        purchaseItems = GetComponentsInChildren<UIAmountItem>().ToList();    
    }

    public override void Set(PackageData info)
    {
        base.Set(info);

        textName.text = info.Name;
        textDes.text = info.Description;
        Instantiate(info.Visual, visualHolder);

        for (int i = 0; i < purchaseItems.Count; i++)
        {
            purchaseItems[i].SetInfo(info.Items[i].item, info.Items[i].amount);
        }
    }
}
