using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [ReadOnly] public bool IsSelected = false;
    public Vector2Int GridPosition { get; private set; }

    [SerializeField] private Color colorStart = Color.red;
    [SerializeField] private Color colorGoal = Color.green;

    public void Init(int x, int y)
    {
        GridPosition = new Vector2Int(x, y);
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


}
