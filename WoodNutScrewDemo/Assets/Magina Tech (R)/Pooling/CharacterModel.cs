using Hung.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour, IModel, IPoolable
{
    [field:SerializeField] public GameObject Model { get; private set; }

    public PoolPartition pooling { get; set; }
    public bool isNewPooling { get; set; }

    public void ToggleOff()
    {
        Model.SetActive(false);
    }

    public void ToggleOn()
    {
        Model.SetActive(true);
    }
}
