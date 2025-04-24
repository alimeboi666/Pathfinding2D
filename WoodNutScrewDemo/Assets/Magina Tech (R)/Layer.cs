using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layer
{
    public const int ENEMY_MASK = 1 << 6 | 1 << 15;

    public const int EXPLOSIVEALBE_MASK = 1 << 6 | 1 << 7 | 1 << 12;

    public const int SHOOTABLE_MASK = ~(1 << 5 | 1 << 30);
}

public static class Tag
{
    public const string TERRAIN = "Terrain";
}