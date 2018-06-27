using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarforgedMonoBehaviour : MonoBehaviour {

    protected void Awake()
    {
        EventHandler._OnGamePreInit += OnGamePreInit;
        EventHandler._OnGameInit += OnGameInit;
        EventHandler._OnGamePostInit += OnGamePostInit;
        EventHandler._OnGameStart += OnGameStart;
        EventHandler._OnTurnStart += OnTurnStart;
        EventHandler._OnTurnEnd += OnTurnEnd;
        EventHandler._OnRoundStart += OnRoundStart;
        EventHandler._OnRoundEnd += OnRoundEnd;
    }


    protected void Start()
    {
        OnInstantiate();
    }

    protected void Update()
    {
        if (GameStateManager.GetGameState().type != GameState.State_Type.LOAD)
        {
            OnUpdate();
        }
    }

    protected virtual void OnGamePreInit() { }
    protected virtual void OnGameInit() { }
    protected virtual void OnGamePostInit() { }
    protected virtual void OnGameStart() { }
    protected virtual void OnTurnStart() { }
    protected virtual void OnTurnEnd() { }
    protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnInstantiate() { }
    protected virtual void OnUpdate() { }

    protected GameState.State_Type GetCurrentState()
    {
        return GameStateManager.GetGameState().type;
    }
}
