using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShower : MonoBehaviour
{
    [SerializeField] List<GameObject> showingList;

    private int extraShowIndex;

    private int currentIndex;

    private bool hasDispel;

    internal GameObject GetElement<T>(T chosen) where T: Enum
    {
        return showingList[(int)(object)chosen];
    }

    internal void ChangeElement<T>(T chosen, GameObject changed) where T: Enum
    {
        showingList[(int)(object)chosen] = changed;
    }

    public void SingleShow<T>(T chosen) where T: Enum
    {
        if (!hasDispel)
        {           
            for (int i = 0; i < showingList.Count; i++)
            {
                if (i != extraShowIndex)
                {
                    showingList[i].SetActive(false);
                }                
            }
            hasDispel = true;
        }

        showingList[currentIndex].SetActive(false);
        showingList[(int)(object)chosen].SetActive(true);
        currentIndex = (int)(object)chosen;
    }

    public void ExtraShow<T>(T chosen, bool isOn = true) where T : Enum
    {
        extraShowIndex = (int)(object)chosen;
        showingList[(int)(object)chosen].SetActive(isOn);
    }
}
