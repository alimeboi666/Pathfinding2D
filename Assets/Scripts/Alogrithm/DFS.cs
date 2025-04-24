using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : IPathfinding
{
    public IEnumerator FindPath(GridManager gridManager, float delay, Action<List<Vector2Int>> onComplete)
    {
        Grid grid = gridManager.Grid;
        Vector2Int start = grid.StartPos;
        Vector2Int goal = grid.GoalPos;

        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        stack.Push(start);
        cameFrom[start] = start;

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Pop();

            if (visited.Contains(current))
                continue;

            visited.Add(current);

            if (current != start && current != goal)
            {
                gridManager.VisualizeVisited(current);
                yield return new WaitForSeconds(delay);
            }

            if (current == goal)
                break;

            foreach (Vector2Int neighbor in GetNeighbors(current, grid))
            {
                if (!visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    if (!cameFrom.ContainsKey(neighbor))
                        cameFrom[neighbor] = current;
                }
            }
        }
        List<Vector2Int> path = new();
        if (!cameFrom.ContainsKey(goal))
        {
            onComplete?.Invoke(null);
            yield break;
        }

        Vector2Int temp = goal;
        while (temp != start)
        {
            path.Add(temp);
            temp = cameFrom[temp];
        }
        path.Add(start);
        path.Reverse();

        onComplete?.Invoke(path);
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos, Grid grid)
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
