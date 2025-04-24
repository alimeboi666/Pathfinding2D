using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputHandle : MonoBehaviour
{
    [SerializeField] private GridEditMode editMode = GridEditMode.None;

    private GridManager gridManager;

    private void Start()
    {
        gridManager = GameManager.Instance.GetGrid();
    }

    private Cell cellTarget;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && editMode != GridEditMode.None)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                Cell clickedCell = hit.collider.GetComponent<Cell>();
                if (clickedCell == cellTarget) return;
                
                cellTarget = clickedCell;

                if (clickedCell != null)
                {
                    switch (editMode)
                    {
                        case GridEditMode.SetStart:
                            gridManager.SetStartPosition(clickedCell);
                            clickedCell.SetColor(GridEditMode.SetStart);
                            break;
                        case GridEditMode.SetGoal:
                            gridManager.SetGoalPosition(clickedCell);
                            clickedCell.SetColor(GridEditMode.SetGoal);
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



}
public enum GridEditMode
{
    None,
    SetStart,
    SetGoal
}