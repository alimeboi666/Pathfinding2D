using Hung.Core;
using Hung.UI.Button;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private Transform gridParent;
    [SerializeField] private SimpleButton generateButton;
    [SerializeField] private SimpleButton findPathButton;
    [SerializeField] private SimpleButton resetButton;
    [SerializeField] private float delayTime = 0.1f; 

    [SerializeField] private GridManager gridManager;


    private bool isReset = false;

    protected override void Awake()
    {

    }

    private void Start()
    {
        //GenerateNewGrid();
        //gridVisualizer.SetupGridVisual();

        generateButton.ClickEvent.AddListener(GenerateNewGrid);
        findPathButton.ClickEvent.AddListener(StartPathfinding);
        resetButton.ClickEvent.AddListener(ResetGrid);
    }

    private void GenerateNewGrid()
    {
        gridManager.SetupGrid();
    }

    private void ResetGrid()
    {
        gridManager.ResetGrid();
    }
    public void StartPathfinding()
    {
        StartCoroutine(Pathfinding.FindPathWithVisualization(gridManager, delayTime, (path) =>
        {
            if (path != null)
            {
                gridManager.VisualizePath(path);
            }
            else
            {
                Debug.LogWarning("Not found path");
            }
        }));
    }

    public GridManager GetGrid()
    {
        return gridManager;
    }
}