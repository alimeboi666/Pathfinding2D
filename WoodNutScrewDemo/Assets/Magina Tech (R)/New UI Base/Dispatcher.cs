using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EventPatching
{
    public BaseVariable main;
    public List<GameEventBase> subs;
}

public class Dispatcher : MonoBehaviour
{
    [SerializeField] private List<EventPatching> eventPatchings;

    
}
