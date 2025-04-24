using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationEvent<T> : SubControl<T>, IAnimationEvent where T: MonoBehaviour, IAnim
{
    public abstract void OnStartAnimationEvent();

    public abstract void OnEndAnimationEvent();
}

public interface IAnimationEvent
{
    void OnStartAnimationEvent();

    void OnEndAnimationEvent();
}