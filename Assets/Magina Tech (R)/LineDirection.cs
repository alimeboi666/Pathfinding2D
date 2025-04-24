using Hung.Pooling;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineDirection : MonoBehaviour, IPoolable
{
    [SerializeField] private SpriteRenderer line;
    [SerializeField] private SpriteRenderer cone;
    [field: ReadOnly, SerializeField] public PoolPartition pooling { get; set; }
    [field: ReadOnly, SerializeField] public bool isNewPooling { get; set; }

    Transform from, to;

    [Button]
    void Test()
    {
        SetLayDown(from.position, to.position);
    }

    Vector3 destination;
    public void SetLayDown(Vector3 from, Vector3 to)
    {
        var dir = to - from;
        if (dir.sqrMagnitude == 0)
        {
            gameObject.SetActive(false);
            return;
        }
        
        destination = to;
        transform.position = (from + to) / 2;
        transform.LookAtDirection3D(dir);
        var size = line.size;
        size.y = Vector3.Distance(from, to)/transform.localScale.y;
        cone.transform.localPosition = 0.5f * size.y * Vector3.forward;
        line.size = size;
    }

    public float SetFrom(Vector3 from)
    {
        transform.position = (from + destination) / 2;
        var size = line.size;
        size.y = Vector3.Distance(from, destination) / transform.localScale.y;
        cone.transform.localPosition = 0.5f * size.y * Vector3.forward;
        line.size = size;
        return size.y;
    }

    public void ToggleOn()
    {
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        gameObject.SetActive(false);
    }
}
