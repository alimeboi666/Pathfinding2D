using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IForceStart: IMono
{
    void OnStart();
}

public interface IForceStart<T>
{
    void OnStart(T caller);
}