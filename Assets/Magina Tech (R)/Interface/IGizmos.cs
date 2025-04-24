using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGizmos: IDebuggable
{
    void OnDrawGizmos();
}

public interface IDebuggable: IMono
{
    bool debugMode { get; set; }
}