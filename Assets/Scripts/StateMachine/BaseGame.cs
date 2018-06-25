using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour {

    public static GameStateManager G_GAMESTATEFSM;


    // Use this for initialization
    void Start () {
        //Set up all data needed to start the game
        CanvasManager.Init();
        CanvasManager.EnableCanvas(CanvasManager.Menu.STATE);
        Player.G_CURRENT_PLAYER = Player.G_PLAYERS[0];
        if (Player.G_CURRENT_PLAYER == null)
        {
            Debug.Log("NO PLAYERS FOUND");
        }
        G_GAMESTATEFSM = new GameStateManager();
        G_GAMESTATEFSM.SetState(new BuildState(Player.Info.PLAYER1));

        //Create all adjacency lists for tiles here
        World.G_WORLD.InitalizeTiles();
    }
}
