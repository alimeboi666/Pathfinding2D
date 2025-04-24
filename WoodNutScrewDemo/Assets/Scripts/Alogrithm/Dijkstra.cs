using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dijkstra : IPathfinding
{
    private class Node
    {
        public Vector2Int position;
        public int gCost; 
        public Node parent;

        public Node(Vector2Int pos)
        {
            position = pos;
            gCost = int.MaxValue;
        }
    }

    public IEnumerator FindPath(GridManager gridManager, float delay, Action<List<Vector2Int>> onComplete)
    {
        Grid grid = gridManager.Grid;

        Dictionary<Vector2Int, Node> nodes = new();
        HashSet<Vector2Int> visited = new();

        Vector2Int start = grid.StartPos;
        Vector2Int goal = grid.GoalPos;

        Node startNode = new Node(start);
        startNode.gCost = 0;
        nodes[start] = startNode;

        PriorityQueue<Node> queue = new PriorityQueue<Node>((a, b) => a.gCost.CompareTo(b.gCost));
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            if (visited.Contains(current.position)) continue;
            visited.Add(current.position);

            if (current.position != start && current.position != goal)
            {
                gridManager.VisualizeVisited(current.position);
                yield return new WaitForSeconds(delay);
            }

            if (current.position == goal)
            {
                List<Vector2Int> path = ReconstructPath(current);
                onComplete?.Invoke(path);
                yield break;
            }

            foreach (var neighborPos in GetNeighbors(current.position, grid))
            {
                if (!nodes.ContainsKey(neighborPos))
                    nodes[neighborPos] = new Node(neighborPos);

                Node neighbor = nodes[neighborPos];
                int newCost = current.gCost + 1;

                if (newCost < neighbor.gCost)
                {
                    neighbor.gCost = newCost;
                    neighbor.parent = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        onComplete?.Invoke(null); 
    }

    private List<Vector2Int> ReconstructPath(Node endNode)
    {
        List<Vector2Int> path = new();
        Node current = endNode;
        while (current != null)
        {
            path.Add(current.position);
            current = current.parent;
        }
        path.Reverse();
        return path;
    }

    private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos, Grid grid)
    {
        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        foreach (var dir in dirs)
        {
            Vector2Int next = pos + dir;
            if (grid.IsValidPosition(next) && grid.Map[next.x, next.y] == 0) 
                yield return next;
        }
    }
}
public class PriorityQueue<T>
{
    private List<T> list = new();
    private Comparison<T> comparison;

    public PriorityQueue(Comparison<T> comparison)
    {
        this.comparison = comparison;
    }

    public void Enqueue(T item)
    {
        list.Add(item);
        list.Sort(comparison);
    }

    public T Dequeue()
    {
        T item = list[0];
        list.RemoveAt(0);
        return item;
    }

    public int Count => list.Count;
}