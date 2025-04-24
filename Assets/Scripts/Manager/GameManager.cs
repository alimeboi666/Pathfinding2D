using Hung.Core;
using Hung.UI.Button;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private PathAlgorithmType algorithm = PathAlgorithmType.AStar;
    [SerializeField] private SimpleButton generateButton;
    [SerializeField] private SimpleButton findPathButton;
    [SerializeField] private SimpleButton resetButton;
    [SerializeField] private float delayTime = 0.1f; 

    [SerializeField] private GridManager gridManager;
    [SerializeField] private TMP_Dropdown algorithmDropdown;

    [SerializeField] private TMP_Text textInfo;

    private bool isReset = false;

    protected override void Awake()
    {

    }

    private void Start()
    {
        InitDropdown();
        //GenerateNewGrid();
        //gridVisualizer.SetupGridVisual();

        generateButton.ClickEvent.AddListener(GenerateNewGrid);
        findPathButton.ClickEvent.AddListener(StartPathfinding);
        resetButton.ClickEvent.AddListener(ResetGrid);
    }

    private void InitDropdown()
    {
        algorithmDropdown.ClearOptions();
        algorithmDropdown.AddOptions(new List<string>(System.Enum.GetNames(typeof(PathAlgorithmType))));

        algorithmDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        algorithmDropdown.value = (int)algorithm; 
        algorithmDropdown.RefreshShownValue();
    }

    private void OnDropdownValueChanged(int value)
    {
        algorithm = (PathAlgorithmType)value;
    }


    private void GenerateNewGrid()
    {
        InputHandle.Instance.ResetCell();
        gridManager.SetupGrid();
    }

    private void ResetGrid()
    {
        InputHandle.Instance.ResetCell();
        gridManager.ResetGrid();
    }
    public void StartPathfinding()
    {
        StartCoroutine(Pathfinding.GetPathfinding(algorithm).FindPath(gridManager, delayTime, (path) =>
        {
            if (path != null)
            {
                gridManager.VisualizePath(path);
                foreach (var item in path)
                {
                    Debug.Log($"<color=yellow>item</color>");
                    Debug.Log("->");
                }

                string pathText = "Path:";

                foreach (var point in path)
                {
                    Debug.Log($"<color=yellow>{point}</color>");
                    pathText += $"->({point.x}, {point.y})";
                }

                textInfo.text = pathText;
            }
            else
            {
                Debug.Log("<color=yellow>Not found path</color>");
                textInfo.text = "Not found path";
            }
        }));
    }



    public GridManager GetGrid()
    {
        return gridManager;
    }
}