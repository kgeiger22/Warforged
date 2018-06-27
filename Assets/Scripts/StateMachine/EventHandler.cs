using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler {

    public delegate void Handler();
    public static event Handler _OnGamePreInit;
    public static event Handler _OnGameInit;
    public static event Handler _OnGamePostInit;
    public static event Handler _OnGameStart;
    public static event Handler _OnTurnStart;
    public static event Handler _OnTurnEnd;
    public static event Handler _OnRoundStart;
    public static event Handler _OnRoundEnd;


    public static void PreInitializeScene()
    {
        if (_OnGamePreInit != null)
            _OnGamePreInit();
    }

    public static void InitializeScene()
    {
        Debug.Log("Initializing Scene");
        if (_OnGameInit != null)
            _OnGameInit();
    }

    public static void PostInitializeScene()
    {
        if (_OnGamePostInit != null)
            _OnGamePostInit();
    }

    public static void StartGame()
    {
        Debug.Log("Starting Game");
        if (_OnGameStart != null)
            _OnGameStart();
    }

    public static void StartTurn()
    {
        if (_OnTurnStart != null)
            _OnTurnStart();
    }

    public static void EndTurn()
    {
        if (_OnTurnEnd != null)
            _OnTurnEnd();
    }

    public static void StartRound()
    {
        if (_OnRoundStart != null)
            _OnRoundStart();
    }

    public static void EndRound()
    {
        if (_OnRoundEnd != null)
            _OnRoundEnd();
    }
}
