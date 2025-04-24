using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Detector<T> : MonoBehaviour, IToggleable
{
    [SerializeField] protected Collider m_collider;
    public Action<T> OnTrigger;

    public event Action<bool> OnVisualChanged;

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out T detected) && OnTrigger != null)
        {
            OnTrigger(detected);
            //if (GameConfig.Instance.detectorDebugMode) Debug.Log(name + " has detected " + LayerMask.LayerToName(other.gameObject.layer));
        }
    }

    public void SetCollider(float size, int collidinglayer = -1)
    {
        if (collidinglayer >= 0) m_collider.gameObject.layer = collidinglayer;
        if (m_collider is BoxCollider box)
        {
            var boxSize = box.size;
            boxSize.x = size;
            box.size = boxSize;
        }
        else if (m_collider is SphereCollider sphere)
        {
            sphere.radius = size;
        }
    }

    public void ToggleOn()
    {
        isVisible = true;
        gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        isVisible = false;
        gameObject.SetActive(false);
    }
}

public class Detector : MonoBehaviour
{
    [SerializeField] protected Collider m_collider;

    public event Action OnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger();
    }
}
