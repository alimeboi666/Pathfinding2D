using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffect3D : DisableByTime, IResizable
{
    static event Action<bool> visualize;
    [field:SerializeField] public GameObject Model { get; private set; }

    TrailRenderer[] m_trails;

    public float size 
    { 
        get => Model.transform.localScale.x;

        set => Model.transform.localScale = Vector3.one * value;
    }

    public float duration
    {
        get => Lifetime;

        set => Lifetime = value;
    }

    public static void ToggleSystem(bool set)
    {
        visualize?.Invoke(set);
    }

    static bool _toggle = true;
    public static void ToggleSystem()
    {
        Debug.Log("visualize: " + visualize.GetInvocationList().Length);
        _toggle = !_toggle;
        visualize?.Invoke(_toggle);      
    }

    void Awake()
    {
        m_trails = GetComponentsInChildren<TrailRenderer>();
    }
    
    void Start()
    {
        if (Model == null) return;
        visualize += Visualize;
        Visualize(_toggle);
    }
    
    void Visualize(bool set)
    {
        if (set) Model.SetActive(true);
        else Model.SetActive(false);
    }

    void OnDisable()
    {
        foreach(var trail in m_trails)
        {
            trail.Clear();
        }
    }

    void OnDestroy()
    {
        visualize -= Visualize;
    }
}
