using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

public class PathfindingSystem : MonoBehaviour
{
    // Map settings
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private GameObject cellPrefab; // Prefab with SpriteRenderer
    [SerializeField] private Transform gridParent;

    // Colors
    [SerializeField] private Color emptyColor = Color.white;
    [SerializeField] private Color wallColor = Color.gray;
    [SerializeField] private Color startColor = Color.green;
    [SerializeField] private Color goalColor = Color.red;
    [SerializeField] private Color pathColor = Color.yellow;

    private int[,] map;
    private GameObject[,] cellObjects;
    private Vector2Int startPos;
    private Vector2Int goalPos;

    private class Node
    {
        public Vector2Int position;
        public int gCost; // Cost from start
        public int hCost; // Heuristic cost to goal
        public int fCost => gCost + hCost;
        public Node parent;

        public Node(Vector2Int pos)
        {
            position = pos;
        }
    }

    void Start()
    {
        GenerateMap();
        SetupGridVisual();
        //FindAndDisplayPath();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        cellObjects = new GameObject[width, height];

        // Initialize map with empty cells
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 0; // Empty
            }
        }

        // Add random walls (30% chance)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Random.value < 0.3f)
                {
                    map[x, y] = 1; // Wall
                }
            }
        }

        // Set start and goal positions
        startPos = new Vector2Int(0, 0);
        goalPos = new Vector2Int(width - 1, height - 1);
        map[startPos.x, startPos.y] = 0; // Ensure start is empty
        map[goalPos.x, goalPos.y] = 0; // Ensure goal is empty
    }

    void SetupGridVisual()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cell = Instantiate(cellPrefab, worldPos, Quaternion.identity, gridParent);
                cellObjects[x, y] = cell;

                SpriteRenderer sr = cell.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (map[x, y] == 1)
                        sr.color = wallColor;
                    else if (x == startPos.x && y == startPos.y)
                        sr.color = startColor;
                    else if (x == goalPos.x && y == goalPos.y)
                        sr.color = goalColor;
                    else
                        sr.color = emptyColor;
                }
            }
        }
    }


    List<Vector2Int> FindPath()
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        Node startNode = new Node(startPos);
        startNode.gCost = 0;
        startNode.hCost = ManhattanDistance(startPos, goalPos);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node current = openList.OrderBy(n => n.fCost).ThenBy(n => n.hCost).First();
            openList.Remove(current);

            if (current.position == goalPos)
            {
                return ReconstructPath(current);
            }

            closedList.Add(current.position);

            foreach (Vector2Int neighborPos in GetNeighbors(current.position))
            {
                if (closedList.Contains(neighborPos))
                    continue;

                int tentativeGCost = current.gCost + 1;
                Node neighbor = openList.Find(n => n.position == neighborPos);

                if (neighbor == null)
                {
                    neighbor = new Node(neighborPos);
                    openList.Add(neighbor);
                }
                else if (tentativeGCost >= neighbor.gCost)
                {
                    continue;
                }

                neighbor.parent = current;
                neighbor.gCost = tentativeGCost;
                neighbor.hCost = ManhattanDistance(neighborPos, goalPos);
            }
        }

        return new List<Vector2Int>(); 
    }

    List<Vector2Int> ReconstructPath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Node current = endNode;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    [Button]
    void FindAndDisplayPath()
    {
        List<Vector2Int> path = FindPath();
        foreach (Vector2Int pos in path)
        {
            if (pos != startPos && pos != goalPos)
            {
                SpriteRenderer sr = cellObjects[pos.x, pos.y].GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = pathColor;
                }
            }
        }
    }

    int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    Vector2Int[] GetNeighbors(Vector2Int pos)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = pos + dir;
            if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < height && map[neighbor.x, neighbor.y] != 1)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors.ToArray();
    }
}