using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : FSM {

    //Global gamestate
    public static GameStateManager G_GAMESTATEFSM;

    public static GameState GetGameState()
    {
        return G_GAMESTATEFSM.state as GameState;
    }

    private void Awake()
    {
        G_GAMESTATEFSM = this;
    }

    private void Start()
    {
        //Set up all data needed to start the game
        CanvasManager.Init();
        CanvasManager.EnableCanvas(CanvasManager.Menu.STATE);
        Player.G_CURRENT_PLAYER = Player.G_PLAYERS[0];
        if (Player.G_CURRENT_PLAYER == null)
        {
            Debug.Log("NO PLAYERS FOUND");
        }
        state = new BuildState(Player.Info.PLAYER1);
        state.Enter();

        //Create all adjacency lists for tiles here
        World.G_WORLD.InitalizeTiles();
    }


}
