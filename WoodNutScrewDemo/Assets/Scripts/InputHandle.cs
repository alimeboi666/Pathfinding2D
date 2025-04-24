using Hung.Core;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputHandle : Singleton<InputHandle>
{
    [SerializeField] private GridEditMode editMode = GridEditMode.None;
    

    private GridManager gridManager;

    private float timeLimitClick = 0.2f;

    private void Start()
    {
        gridManager = GameManager.Instance.GetGrid();
    }

    public Cell cellStart;
    public Cell cellGoal;

    private float clickCooldown;
    private void Update()
    {
        clickCooldown -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && editMode != GridEditMode.None && clickCooldown <= 0)
        {
            clickCooldown = timeLimitClick;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Cell clickedCell = hit.collider.GetComponent<Cell>();

                

                if ((editMode == GridEditMode.SetStart && clickedCell == cellStart) ||
                (editMode == GridEditMode.SetGoal && clickedCell == cellGoal) || !clickedCell.IsFreeCell())
                {
                    return;
                }


                if (clickedCell != null)
                {
                    switch (editMode)
                    {
                        case GridEditMode.SetStart:
                            if (cellStart != null)
                                cellStart.OnReset();
                            cellStart = clickedCell;
                            gridManager.SetStartPosition(clickedCell);
                            clickedCell.SetColor(GridEditMode.SetStart);
                            editMode = GridEditMode.SetGoal;
                            break;
                        case GridEditMode.SetGoal:
                            if (cellGoal != null)
                                cellGoal.OnReset();
                            cellGoal = clickedCell;
                            gridManager.SetGoalPosition(clickedCell);
                            clickedCell.SetColor(GridEditMode.SetGoal);
                            editMode = GridEditMode.SetStart;
                            //gridManager.RemovePath();
                            break;
                    }
                }
            }
        }
    }




    public void SetEditMode(int mode)
    {
        editMode = (GridEditMode)mode;
    }

    public void ResetCell()
    {
        if (cellStart != null)
            cellStart.OnReset();
        if (cellGoal != null)
            cellGoal.OnReset();
        cellStart = null;
        cellGoal = null;
        editMode = GridEditMode.SetStart;
    }


}
public enum GridEditMode
{
    None,
    SetStart,
    SetGoal
}