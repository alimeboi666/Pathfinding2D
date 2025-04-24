using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hung.GameData;
using Hung.StatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

public interface ICore
{
    void Init();
}

public interface ICore<T> where T: IIdentifiedData
{
    T data { get; }
    void Init(T data);
}

public abstract class Quest : ICore
{
    [field:SerializeField] public bool trackData { get; protected set; }

    public abstract void Goto(GameData targetData);

    protected abstract void CustomAssign(IEffectable amount, GameData targetData);

    Action<int> onValueChanged;
    [InlineButton("IncreaseTrack"), SerializeField] protected int current;

    [Button]
    public virtual int Track(GameData targetData)
    {
        return current;
    }
    void IncreaseTrack()
    {
        current++;
        onValueChanged?.Invoke(current);
    }

    public void AssignListener(IEffectable amount, GameData targetData)
    {
        new AmountUp(Track(targetData)).Set(amount);
        onValueChanged += (int value) => new AmountUp(value).Set(amount);
        CustomAssign(amount, targetData);
    }

    public virtual void Init()
    {

    }

    public virtual void Reset(GameData targetData)
    {

    }
}

[CreateAssetMenu(menuName = "Hung/Data/Quest")]
public class QuestData : GameData
{
    [field: SerializeReference] public Quest Core { get; private set; }

    public override void Init()
    {
        base.Init();
        Core.Init();
    }
}