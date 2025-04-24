using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
  
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [Range(0, 1), SerializeField] private float spacing = 0.1f;
    [Range(0, 2), SerializeField] private float cellSize = 0.1f;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform gridParent;
    
    
    
    private Grid grid;
    public Grid Grid => grid;

    private Cell[,] cellObjects;

    private Queue<Cell> cellPool = new Queue<Cell>();
    private List<Cell> activeCells = new List<Cell>();


    [SerializeField] private GridConfig gridConfig;


    private void Start()
    {
        Init();
    }


    public void Init()
    {
        CalculateCellSizeToFitScreen();
        grid = new Grid(width, height);
        grid.GenerateRandomMap();
        ResizePoolIfNeeded(); 
        cellObjects = new Cell[width, height];
    }

    public void SetupGrid()
    {
        Init();

        foreach (var cell in activeCells)
        {
            cell.gameObject.SetActive(false);
            cellPool.Enqueue(cell);
        }
        activeCells.Clear();

        float totalSize = cellSize + spacing;
        float gridWorldWidth = width * totalSize;
        float gridWorldHeight = height * totalSize;
        Vector3 origin = new Vector3(-gridWorldWidth / 2f + totalSize / 2f, -gridWorldHeight / 2f + totalSize / 2f, 0);

       
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = GetPooledCell();
                cell.transform.SetParent(gridParent);
                cell.transform.position = new Vector3(x * totalSize, y * totalSize, 0) + origin;
                cell.transform.localScale = new Vector3(cellSize, cellSize, 1f);
                cell.gameObject.SetActive(true);

                activeCells.Add(cell);
                cellObjects[x, y] = cell;

                int weight = 0;

                SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (grid.Map[x, y] == 1)
                    {
                        weight = 1;
                        sr.color = gridConfig.WallColor;
                    }
                                   
                    else if (x == grid.StartPos.x && y == grid.StartPos.y)
                        sr.color = gridConfig.StartColor;
                    else if (x == grid.GoalPos.x && y == grid.GoalPos.y)
                        sr.color = gridConfig.GoalColor;
                    else
                        sr.color = gridConfig.EmptyColor;
                }

                cell.Init(x, y, weight);
            }
        }
    }


    private Cell GetPooledCell()
    {
        if (cellPool.Count > 0)
        {
            Cell cell = cellPool.Dequeue();
            cell.gameObject.SetActive(true);
            return cell;
        }

        Cell newCell = Instantiate(cellPrefab, gridParent);
        return newCell;
    }


    private void ResizePoolIfNeeded()
    {
        int needed = width * height;
        int totalAvailable = cellPool.Count + activeCells.Count;

        if (totalAvailable < needed)
        {
            int toAdd = needed - totalAvailable;
            for (int i = 0; i < toAdd; i++)
            {
                Cell cell = Instantiate(cellPrefab, gridParent);
                cell.gameObject.SetActive(false);
                cellPool.Enqueue(cell);
            }
        }
    }

    private List<Cell> visitedCellList = new();

    public void VisualizeVisited(Vector2Int pos)
    {
        if (pos != grid.StartPos && pos != grid.GoalPos)
        {
            SpriteRenderer sr = cellObjects[pos.x, pos.y].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = gridConfig.VisitedColor;
               
            }
            visitedCellList.Add(cellObjects[pos.x, pos.y]);
        }
    }

    public void VisualizePath(List<Vector2Int> path)
    {

        foreach (Vector2Int pos in path)
        {
            if (pos != grid.StartPos && pos != grid.GoalPos)
            {
                SpriteRenderer sr = cellObjects[pos.x, pos.y].GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = gridConfig.PathColor;
                }
            }
        }
    }

    public void ResetGrid()
    {
        grid.StartPos = new Vector2Int(-1, -1);
        grid.GoalPos = new Vector2Int(-1, -1);

        RemovePath();

    }

    public void RemovePath()
    {
        foreach (Cell cell in visitedCellList)
        {
            SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
            sr.color = gridConfig.EmptyColor;
        }
        visitedCellList.Clear();
    }

    public void SetStartPosition(Cell cell)
    {
        cell.SetColor(GridEditMode.None);
        grid.StartPos = cell.GridPosition;
    }

    public void SetGoalPosition(Cell cell)
    {
        cell.SetColor(GridEditMode.None);
        grid.GoalPos = cell.GridPosition;

        //SetupGrid(); 
    }


    public Vector2Int WorldToGridPosition(Vector2 worldPos)
    {
        float totalSize = cellSize + spacing;
        float gridWorldWidth = width * totalSize;
        float gridWorldHeight = height * totalSize;
        Vector2 origin = new Vector3(-gridWorldWidth / 2f + totalSize / 2f, -gridWorldHeight / 2f + totalSize / 2f, 0);

        Vector2 localPos = worldPos - origin;
        int x = Mathf.FloorToInt(localPos.x / totalSize);
        int y = Mathf.FloorToInt(localPos.y / totalSize);

        return new Vector2Int(x, y);
    }


    private void CalculateCellSizeToFitScreen()
    {
        Camera cam = Camera.main;

        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect * 0.9f;

        float totalSpacingX = spacing * (width - 1);
        float totalSpacingY = spacing * (height - 1);

        float maxCellWidth = (screenWidth - totalSpacingX) / width;
        float maxCellHeight = (screenHeight - totalSpacingY) / height;

        cellSize = Mathf.Min(maxCellWidth, maxCellHeight);
    }

}

[System.Serializable]
public struct GridConfig
{
    [field: SerializeField] public Color EmptyColor { get; private set; }
    [field: SerializeField] public Color WallColor { get; private set; }
    [field: SerializeField] public Color StartColor { get; private set; }
    [field: SerializeField] public Color GoalColor { get; private set; }
    [field: SerializeField] public Color PathColor { get; private set; }
    [field: SerializeField] public Color VisitedColor { get; private set; }
}
