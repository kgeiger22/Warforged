using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGame : MonoBehaviour {
    
    public static int round_number = 0;

    // Use this for initialization
    void Start () {
        Global.GAMESTATEFSM = new GameStateFSM();
        Global.GAMESTATEFSM.SetState(new LoadState());
        CanvasManager.Init();
        CanvasManager.EnableCanvas(CanvasManager.Menu.STATE);
        EventHandler.PreInitializeScene();
        EventHandler.InitializeScene();
        EventHandler.PostInitializeScene();
        Global.GAMESTATEFSM.NextState();
        EventHandler.StartGame();
    }
}
