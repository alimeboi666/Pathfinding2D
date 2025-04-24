using System.Collections;
using System.Collections.Generic;
using Hung.Pooling;
using UnityEngine;

public class Point : MonoBehaviour, ICommonPoolable, IModel
{
    public PoolPartition pooling { get; set; }
    public bool isNewPooling { get; set ; }

    [field:SerializeField] public GameObject Model { get; private set; }

    private void Awake()
    {
        Model.SetActive(false);
    }
}
