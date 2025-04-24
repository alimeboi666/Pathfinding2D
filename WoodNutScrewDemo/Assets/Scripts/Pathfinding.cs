using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinding
{
    private class Node
    {
        public Vector2Int position;
        public int gCost;
        public int hCost;
        public int fCost => gCost + hCost;
        public Node parent;

        public Node(Vector2Int pos) => position = pos;
    }

    public static IEnumerator FindPathWithVisualization(GridManager gridManager, float delay, System.Action<List<Vector2Int>> onComplete)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

        Grid grid = gridManager.Grid;

        Node startNode = new Node(grid.StartPos)
        {
            gCost = 0,
            hCost = ManhattanDistance(grid.StartPos, grid.GoalPos)
        };
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node current = openList.OrderBy(n => n.fCost).ThenBy(n => n.hCost).First();
            openList.Remove(current);

            if (current.position != grid.StartPos && current.position != grid.GoalPos)
            {
                gridManager.VisualizeVisited(current.position);
                yield return new WaitForSeconds(delay);
            }

            if (current.position == grid.GoalPos)
            {
                List<Vector2Int> path = ReconstructPath(current);
                onComplete?.Invoke(path);
                yield break;
            }

            closedList.Add(current.position);

            foreach (Vector2Int neighborPos in GetNeighbors(current.position, grid))
            {
                if (closedList.Contains(neighborPos)) continue;

                int tentativeGCost = current.gCost + 1;
                Node neighbor = openList.FirstOrDefault(n => n.position == neighborPos);

                if (neighbor == null)
                {
                    neighbor = new Node(neighborPos);
                    openList.Add(neighbor);
                }
                else if (tentativeGCost >= neighbor.gCost)
                {
                    continue;
                }

                neighbor.gCost = tentativeGCost;
                neighbor.hCost = ManhattanDistance(neighborPos, grid.GoalPos);
                neighbor.parent = current;
            }
        }

        onComplete?.Invoke(null); 
    }

    private static List<Vector2Int> ReconstructPath(Node endNode)
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

    private static int ManhattanDistance(Vector2Int a, Vector2Int b) =>
        Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

    private static IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos, Grid grid)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        foreach (var dir in dirs)
        {
            Vector2Int next = pos + dir;
            if (grid.IsValidPosition(next))
                yield return next;
        }
    }
}
