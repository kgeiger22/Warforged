using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour {

    public static GameStateManager G_GAMESTATEFSM;


    // Use this for initialization
    void Start () {
        G_GAMESTATEFSM = new GameStateManager();
        G_GAMESTATEFSM.SetState(new LoadState());
        CanvasManager.Init();
        CanvasManager.EnableCanvas(CanvasManager.Menu.STATE);
        EventHandler.PreInitializeScene();
        EventHandler.InitializeScene();
        EventHandler.PostInitializeScene();
        G_GAMESTATEFSM.NextState();
        EventHandler.StartGame();
    }
}
