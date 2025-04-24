using System;
using Hung.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class PoolAmmo : MonoBehaviour, ICast<Vector3, Transform>, IPoolable, IToggleable
{
    [SerializeField] private float lifetime;
    [ReadOnly] [SerializeField] protected float _timer;

    public event Action<bool> OnVisualChanged;

    public float Lifetime 
    { 
        get            
        {
            if (lifetime == 0)
            {
                lifetime = 3;
            }
            return lifetime;
        }
        set
        {
            lifetime = value;
            _timer = lifetime;
        }
    }

    public PoolPartition pooling { get; set; }

    public bool isNewPooling { get; set; }

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    private void OnEnable()
    {
        ResetLifetime();
    }

    public void Cast(Vector3 position, Transform parent)
    {
        transform.SetParent(parent, false);
        transform.position = position;
        ToggleOn();  
    }

    public void Cast(Vector3 position)
    {
        transform.position = position;
        ToggleOn();
    }

    public void Precast()
    {
        ToggleOff();
    }

    internal virtual void ResetLifetime()
    {
        _timer = Lifetime;
    }

    void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            Pool.BackToPool(this);
        }
    }

    public void ToggleOn()
    {
        isVisible = true;
        gameObject?.SetActive(true);
    }

    public void ToggleOff()
    {
        isVisible = false;
        gameObject?.SetActive(false);
    }
}
