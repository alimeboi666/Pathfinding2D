using System;
using System.Collections.Generic;
using System.Linq;
using Hung.Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class ObjectFollower<T> : ObjectFollower where T: IFollowable
{
    internal void SetFollow(T follow)
    {
        SetTarget(follow.followTarget);
        gameObject.SetActive(true);
        OnFollow(follow);
    }

    protected abstract void OnFollow(T follow);

    internal abstract void Unfollow(Action onComplete = null);
}

public class ObjectFollower : WorldUI, IModel, IFollow, IToggleable
{
    [field: SerializeField] public Transform target { get; set; }
    [field: SerializeField] public Transform current { get; set; }

    [SerializeField] private bool startStatic;
    [SerializeField] private bool startEnable;

    protected delegate bool VisibleChecker();

    protected VisibleChecker VisibleCheck;

    Action following;

    public event Action<bool> OnVisualChanged;

    [field: SerializeField] public GameObject Model { get; private set; }

    [field: ReadOnly, SerializeField] public bool isVisible { get; private set; }

    public void SetTarget(Transform target)
    {
        this.target = target;
        InstantLocateAt(target.position);
    }

    protected new void Awake()
    {
        base.Awake();
        if (!startStatic)
        {
            if (startEnable)
            {
                StartFollow();
            }
            else
            {
                StartHidden();
            }
        }
        else
        {
            following = () => { };
        }
        //following = () => { };
    }

    void Update()
    {
        following();
    }

    protected virtual void OnFollowing()
    {
        LocateAt(target.position);
    }

    public void OnFollow()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            if (VisibleCheck != null && !VisibleCheck())
            {
                StartHidden();
            }
            else OnFollowing();
        }
        else 
        {
            StartHidden();           
        }
    }

    public void OnHidden()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            if (VisibleCheck == null || VisibleCheck())
            {
                StartFollow();               
            }           
        }
    }

    protected virtual void StartFollow()
    {
        InstantLocateAt(target.position);
        OnFollow();
        ToggleOn();
        following = OnFollow;
    }

    protected virtual void StartHidden()
    {
        ToggleOff();
        following = OnHidden;
    }

    [Button]
    public override void ToggleOn()
    {
        isVisible = true;
        Model.SetActive(true);
    }

    [Button]
    public override void ToggleOff()
    {
        isVisible = false;
        Model.SetActive(false);
    }
}

public interface IFollow<T> where T: IFollowable
{
    T target { get; set; }

    Vector3 Offset { get; set; }
}

public interface IFollow
{
    Transform target { get; set; }

    Vector3 Offset { get; set; }
}

public interface IFollowable: IMono
{
    Transform followTarget { get; }
}

public interface IFollow2
{
    Transform firstTarget { get; set; }

    Transform secondTarget { get; set; }

    Vector3 offset { get; set; }
}