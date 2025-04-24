using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModel: IMono
{
    GameObject Model { get; }
}