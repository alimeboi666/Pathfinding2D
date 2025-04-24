using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExecute
{
    void Execute(float value);
}
public interface IExecute<T> where T : Object
{
    void Execute(T caller);
}