using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Hung.Core
{
    public enum ExecuteLayer
    {
        Default = 0,
        Early = -1,
        Late = 1
    }

    public interface IExecuteFlow
    {
        ExecuteLayer ExecuteLayer { get; }
    }

    public interface IGameplayFlow : IFlow, ISingletonRole, IGameplayLoad, IGameplayStart, IGameplayPrepareStart, IGameplayLoop, IGameplayPrepareEnd, IGameplayEnd, IGameplayInterupt, ICache
    {
        GameStateInfo stateInfo { get; }

        void StartGame();
    }

    public interface IGameplayLoad : IExecuteFlow, IMono
    {
        void OnGameplayLoad();
    }

    public interface IGameplayPrepareStart : IExecuteFlow, IMono
    {
        void OnGameplayPrepareStart();
    }

    public interface IGameplayStart : IExecuteFlow, IMono
    {
        void OnGameplayStart();
    }

    public interface IGameplayLoop : IExecuteFlow, IMono
    {
        void OnGameplayLoop();
    }

    public interface IGameplayPrepareEnd : IExecuteFlow, IMono
    {
        void OnGameplayPrepareEnd(GameStateInfo stateInfo);
    }

    public interface IGameplayEnd : IExecuteFlow, IMono
    {
        void OnGameplayEnd(GameStateInfo stateInfo);
    }

    public interface IGameplayInterupt : IExecuteFlow, IMono
    {
        void OnGameplayInterupt();
    }

    public interface IGameStateInfo
    {

    }
}