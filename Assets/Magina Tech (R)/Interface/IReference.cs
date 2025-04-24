//using Cinemachine;              
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReference: IMono
{
    
}

public interface ICineCamera: ICamera
{
    //CinemachineVirtualCamera virtualCamera { get; }
}

public interface ICamera : IReference
{
    Camera targetCamera { get; }
}

public interface ICanvas: IReference
{
    Canvas targetCanvas { get; }
}