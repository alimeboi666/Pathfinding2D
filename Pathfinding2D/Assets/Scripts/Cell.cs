using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [ReadOnly] public bool IsSelected = false;
    public Vector2Int GridPosition { get; private set; }

    [SerializeField] private int weight;
    [SerializeField] private Color colorStart = Color.green;
    [SerializeField] private Color colorGoal = Color.red;

    public void Init(int x, int y, int num)
    {
        GridPosition = new Vector2Int(x, y);
        weight = num;
    }
    public void SetColor(GridEditMode mode)
    {
        var sr = GetComponent<SpriteRenderer>();
        if (IsSelected)
        {
            sr.color = Color.white;
            IsSelected = false;
            return;
        }
        switch (mode)
        {
            case GridEditMode.SetStart:
                sr.color = colorStart;
                break;
            case GridEditMode.SetGoal:
                sr.color = colorGoal;
                break;
            default:
                sr.color = Color.white;
                break;
        }
    }

    public void OnReset()
    {
        IsSelected = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        weight = 0;
    }


    public bool IsFreeCell()
    {
        return weight == 0;
    }

}
