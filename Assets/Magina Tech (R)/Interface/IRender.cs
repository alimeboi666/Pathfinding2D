using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOffscreenFollowable: ICollider, IFollowable
{

}

public interface IRenderer: IMono
{
    Renderer Renderer { get; }
}

public interface ICollider
{
    Collider Collider { get; }
}