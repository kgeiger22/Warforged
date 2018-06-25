using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : FSM {


    public static GameState GetGameState()
    {
        return BaseGame.G_GAMESTATEFSM.state as GameState;
    }

    public GameStateManager()
    {
        BaseGame.G_GAMESTATEFSM = this;
    }


}
