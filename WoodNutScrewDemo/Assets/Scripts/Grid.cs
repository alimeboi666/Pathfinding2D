using UnityEngine;

public class Grid
{
    private int width;
    private int height;
    private int[,] map;
    private Vector2Int startPos;
    private Vector2Int goalPos;

    public int Width => width;
    public int Height => height;
    public int[,] Map => map;
    public Vector2Int StartPos
    {
        get => startPos;
        set => startPos = value;
    }
    public Vector2Int GoalPos
    {
        get => goalPos;
        set => goalPos = value;
    }

    public Grid(int width, int height)
    {
        this.width = width;
        this.height = height;
        map = new int[width, height];
    }

    public void GenerateRandomMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = Random.value < 0.3f ? 1 : 0; 
            }
        }

        startPos = new Vector2Int(0, 0);
        goalPos = new Vector2Int(width - 1, height - 1);
        map[startPos.x, startPos.y] = 0;
        map[goalPos.x, goalPos.y] = 0;
    }

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height && map[pos.x, pos.y] != 1;
    }
}