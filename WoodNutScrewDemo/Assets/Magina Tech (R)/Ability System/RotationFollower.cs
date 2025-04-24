using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollower : MonoBehaviour, IFollow, IPoolCleanup
{
    public Transform target { get; set; }
    public Vector3 Offset { get; set; }

    void LateUpdate()
    {
        transform.eulerAngles = target.eulerAngles + Offset;
    }

    public void Dispose()
    {
        Destroy(this);
    }
}
