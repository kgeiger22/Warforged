using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateFSM : FSM {


    public static GameState GetGameState()
    {
        return Global.GAMESTATEFSM.state as GameState;
    }

    public GameStateFSM()
    {
        Global.GAMESTATEFSM = this;
    }


}
