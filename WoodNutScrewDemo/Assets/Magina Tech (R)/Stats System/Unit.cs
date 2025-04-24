using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Hung.StatSystem.Binding;
using UnityEngine.Events;

namespace Hung.GameData.RPG
{
    public enum UnitStat : byte
    {
        ATK,
        HP,
        Armor,
        Range,
        MS,
        EVA,
        CritChance,
        CritDMG,
        Price,
        Level,
        Amount,
        Cooldown,
        Energy,
        Fury,
        Quantity
    }


    public interface IStatHolder<T> where T : IStat
    {
        T StatHolder { get; }
    }

}

namespace Hung.Unit
{
    public interface IUnitFlow: IFlow
    {
        UnityEvent<IUnit> startDead { get; }

        UnityEvent<IUnit> finishDead { get; }

        void StartDead();

        void FinishDead();

        void Hit();
    }

    public interface IUnit : IEffectable, IModel, IUnitFlow, IFollowable, IHP, IPosition
    {
        IUnit currentTarget { get; set; }

        int teamID { get; }

        float visualSize { get; }

        Vector3 faceDirection { get; }

        bool isDead { get; }

        void Respawn(UnitBindingStat buildingStat);
    }
}

namespace Hung
{
    public interface IPosition
    {
        Vector3 positionValue { get; }
    }

}