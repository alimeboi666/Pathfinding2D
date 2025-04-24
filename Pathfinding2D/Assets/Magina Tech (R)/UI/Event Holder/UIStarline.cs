using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;

public class UIStarline : UI_EventHolder
{
    [ReadOnly, SerializeField] private int _pickedNumber;
    [SerializeField] private Image[] stars;

    int _index;
    int i;
    protected override bool ClickAction(PointerEventData eventData, GameObject clicked)
    {
        _index = clicked.transform.GetSiblingIndex();

        SetStarCount(_index);
        return true;
    }

    private void SetStarAt(int index, bool set)
    {       
        stars[index].color = set ? Color.yellow : Color.gray;
    }

    public void SetStarCount(int index)
    {
        index = Mathf.Clamp(index, 0, stars.Length - 1);
        for (i = 0; i <= index; i++)
        {
            SetStarAt(i, true);
        }
        for (i = index + 1; i < _pickedNumber; i++)
        {
            SetStarAt(i, false);
        }
        _pickedNumber = index + 1;
    }
}
