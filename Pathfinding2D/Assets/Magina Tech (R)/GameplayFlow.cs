using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using System;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

namespace Hung.Core
{
    public class GameplayFlow : SerializedMonoBehaviour, IGameplayFlow
    {
        public ExecuteLayer ExecuteLayer => ExecuteLayer.Early;

        [field: SerializeField] public GameStateInfo stateInfo { get; private set; }

        [field: ReadOnly, NonSerialized, OdinSerialize] private List<IGameplayLoad> GameplayLoaders {get; set;}

        [field: ReadOnly, NonSerialized, OdinSerialize] private List<IGameplayPrepareStart> GameplayPrepareStarters { get; set; }

        [field: ReadOnly, NonSerialized, OdinSerialize] private List<IGameplayStart> GameplayStarters { get; set; }

        [field: ReadOnly, NonSerialized, OdinSerialize] private List<IGameplayPrepareEnd> GameplayPrepareEnders { get; set; }

        [field: ReadOnly, NonSerialized, OdinSerialize] private List<IGameplayEnd> GameplayEnders { get; set; }

        [field: ReadOnly, NonSerialized, OdinSerialize] private List<IGameplayInterupt> GameplayInterupters { get; set; }


        public void OnCached()
        {
            GameplayLoaders = this.FindMultiComponents<IGameplayLoad>(isIncludeInactive: false, ignoreSelf: true)
                .Where(i =>
                {
                    if (i is ISingletonRole) return true;
                    else
                    {
                        Debug.LogError($"{i.gameObject.name} is not a singleton but get derived from IExecuteFlow interface");
                        return false;
                    }
                })
                .OrderBy(i => (int)i.ExecuteLayer).ToList();

            GameplayPrepareStarters = this.FindMultiComponents<IGameplayPrepareStart>(isIncludeInactive: false, ignoreSelf: true)
                .Where(i =>
                {
                    if (i is ISingletonRole) return true;
                    else
                    {
                        Debug.LogError($"{i.gameObject.name} is not a singleton but get derived from IExecuteFlow interface");
                        return false;
                    }
                }).OrderBy(i => (int)i.ExecuteLayer).ToList();

            GameplayStarters = this.FindMultiComponents<IGameplayStart>(isIncludeInactive: false, ignoreSelf: true)
                .Where(i =>
                {
                    if (i is ISingletonRole) return true;
                    else
                    {
                        Debug.LogError($"{i.gameObject.name} is not a singleton but get derived from IExecuteFlow interface");
                        return false;
                    }
                }).OrderBy(i => (int)i.ExecuteLayer).ToList();

            GameplayPrepareEnders = this.FindMultiComponents<IGameplayPrepareEnd>(isIncludeInactive: false, ignoreSelf: true)
                .Where(i =>
                {
                    if (i is ISingletonRole) return true;
                    else
                    {
                        Debug.LogError($"{i.gameObject.name} is not a singleton but get derived from IExecuteFlow interface");
                        return false;
                    }
                }).OrderBy(i => (int)i.ExecuteLayer).ToList();

            GameplayEnders = this.FindMultiComponents<IGameplayEnd>(isIncludeInactive: false, ignoreSelf: true)
                .Where(i =>
                {
                    if (i is ISingletonRole) return true;
                    else
                    {
                        Debug.LogError($"{i.gameObject.name} is not a singleton but get derived from IExecuteFlow interface");
                        return false;
                    }
                }).OrderBy(i => (int)i.ExecuteLayer).ToList();

            GameplayInterupters = this.FindMultiComponents<IGameplayInterupt>(isIncludeInactive: false, ignoreSelf: true)
                .Where(i =>
                {
                    if (i is ISingletonRole) return true;
                    else
                    {
                        Debug.LogError($"{i.gameObject.name} is not a singleton but get derived from IExecuteFlow interface");
                        return false;
                    }
                }).OrderBy(i => (int)i.ExecuteLayer).ToList();
        }

        private void Awake()
        {
            hasStarted = false;
        }

        [ReadOnly, SerializeField] bool hasStarted;
        public void StartGame()
        {
            if (!hasStarted)
            {
                hasStarted = true;
                Archetype.GameFlow.OnGameplayLoad();
                Archetype.GameFlow.OnGameplayPrepareStart();
                Archetype.GameFlow.OnGameplayStart();
            }           
        }

        [Button]
        public void ForceStart()
        {
            hasStarted = true;
            Archetype.GameFlow.OnGameplayLoad();
            Archetype.GameFlow.OnGameplayPrepareStart();
            Archetype.GameFlow.OnGameplayStart();
        }

        public void OnGameplayLoad()
        {
            Debug.Log("<color=yellow>[GAME CORE]: Gameplay Load</color>");
            foreach (IGameplayLoad loader in GameplayLoaders)
            {
                if (loader.gameObject.activeInHierarchy) loader.OnGameplayLoad();
                else Debug.LogWarning($"Gameplay Loader: {loader.gameObject.name} is not active");
            };
        }

        public void OnGameplayPrepareStart()
        {
            Debug.Log("<color=yellow>[GAME CORE]: Gameplay Prepare Start</color>");
            stateInfo = new(currentState: GameState.OnGoing);
            foreach (IGameplayPrepareStart prepareStarter in GameplayPrepareStarters)
            {
                if (prepareStarter.gameObject.activeInHierarchy) prepareStarter.OnGameplayPrepareStart();
                else Debug.LogWarning($"Gameplay Prepare Starter: {prepareStarter.gameObject.name} is not active");
            };
        }

        public void OnGameplayStart()
        {
            Debug.Log("<color=yellow>[GAME CORE]: Gameplay Start</color>");
            foreach (IGameplayStart starter in GameplayStarters)
            {
                if (starter.gameObject.activeInHierarchy) starter.OnGameplayStart();
                else Debug.LogWarning($"Gameplay Starter: {starter.gameObject.name} is not active");
            };
        }

        public void OnGameplayLoop()
        {

        }

        public async void OnGameplayPrepareEnd(GameStateInfo stateInfo)
        {
            if (!isAlreadyEnd)
            {
                isAlreadyEnd = true;
                this.stateInfo = stateInfo;
                foreach (IGameplayPrepareEnd prepareEnder in GameplayPrepareEnders)
                {
                    if (prepareEnder.gameObject.activeInHierarchy) prepareEnder.OnGameplayPrepareEnd(stateInfo);
                    else Debug.LogWarning($"Gameplay Prepare Ender: {prepareEnder.gameObject.name} is not active");
                };

                await Task.Delay(1800);

                OnGameplayEnd(stateInfo);
            }
        }


        private bool isAlreadyEnd;
        public void OnGameplayEnd(GameStateInfo stateInfo)
        {
            this.stateInfo = stateInfo;
            foreach (IGameplayEnd ender in GameplayEnders)
            {
                if (ender.gameObject.activeInHierarchy) ender.OnGameplayEnd(stateInfo);
                else Debug.LogWarning($"Gameplay Ender: {ender.gameObject.name} is not active");
            };
        }

        public void OnGameplayInterupt()
        {
            foreach (IGameplayInterupt interupter in GameplayInterupters)
            {
                if (interupter.gameObject.activeInHierarchy) interupter.OnGameplayInterupt();
                else Debug.LogWarning($"Gameplay Interupter: {interupter.gameObject.name} is not active");
            };
        }
    }


  
}