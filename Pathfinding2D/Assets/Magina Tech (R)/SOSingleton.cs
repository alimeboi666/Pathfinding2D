using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SOSingleton<T> : SerializedScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.Log(typeof(T).ToString());
                _instance = Resources.Load(typeof(T).ToString()) as T;
                if (_instance == null) Debug.Log("Create the " + typeof(T).ToString() + "!");
            }
            return _instance;
        }
    }
}

