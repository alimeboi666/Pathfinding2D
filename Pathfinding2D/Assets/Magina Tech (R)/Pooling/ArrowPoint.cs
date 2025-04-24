using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPoint : DisableByTime, ICast<Vector3, Vector3>
{
    public void Cast(Vector3 position, Vector3 direction)
    {
        Cast(position);
        transform.LookAtDirection3D(direction);
    }

    public void Cast(Ray ray)
    {
        Cast(ray.origin, ray.direction);
    }
}
