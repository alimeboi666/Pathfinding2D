using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.Core
{

    [Serializable]
    public struct GameStateInfo : IGameStateInfo
    {
        public string message;
        public GameState current;

        public GameStateInfo(string message = "", GameState currentState = GameState.OnGoing)
        {
            this.message = message;
            this.current = currentState;
        }

        public void SetState(GameState state)
        {
            this.message = state.ToString();
            this.current = state;
        }
    }
}
