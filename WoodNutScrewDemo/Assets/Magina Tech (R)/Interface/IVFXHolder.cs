using Hung.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVFXHolder<T> where T: System.Enum 
{
    void ApplyVFXTo(VisualEffect3D vfx, T part);
}
