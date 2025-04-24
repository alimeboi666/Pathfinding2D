using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFS : IPathfinding
{
    public IEnumerator FindPath(GridManager gridManager, float delay, Action<List<Vector2Int>> onComplete)
    {
        Grid grid = gridManager.Grid;
        Vector2Int start = grid.StartPos;
        Vector2Int goal = grid.GoalPos;

        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        frontier.Enqueue(start);
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            Vector2Int current = frontier.Dequeue();

            if (current != start && current != goal)
            {
                gridManager.VisualizeVisited(current);
                yield return new WaitForSeconds(delay);
            }

            if (current == goal)
                break;

            foreach (Vector2Int neighbor in GetNeighbors(current, grid))
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
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
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Vector2Int neighbor = pos + dir;
            if (grid.IsValidPosition(neighbor))
                yield return neighbor;
        }
    }
}
