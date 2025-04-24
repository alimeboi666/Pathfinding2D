using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinding
{
    public static IPathfinding GetPathfinding(PathAlgorithmType type)
    {
        switch (type)
        {
            case PathAlgorithmType.AStar:
                return new AStar();
            case PathAlgorithmType.Dijkstra:
                return new Dijkstra();
            case PathAlgorithmType.BFS:
                return new BFS();
            case PathAlgorithmType.DFS:
                return new DFS();
            default:
                Debug.LogError("Unknown PathAlgorithmType: " + type);
                return null;
        }
    }


}
public enum PathAlgorithmType
{ 
    AStar,
    Dijkstra,
    BFS,
    DFS
}
public interface IPathfinding
{
    IEnumerator FindPath(GridManager gridManager, float delay, System.Action<List<Vector2Int>> onComplete);
}