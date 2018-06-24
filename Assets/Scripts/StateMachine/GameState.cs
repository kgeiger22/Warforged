using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : FSM_State {

    public enum State_Type
    {
        BEGIN,
        BUILD,
        TURN,
        END,   
    }

    public Player.Info player_info { get; protected set; }
    public State_Type type { get; protected set; }
}

public class BuildState : GameState
{
    public BuildState(Player.Info _info)
    {
        player_info = _info;
    }
    public override void Enter()
    {
        Player.SetPlayer(player_info);
        CanvasManager.EnableCanvas(CanvasManager.Menu.STORE);
        type = State_Type.BUILD;
        switch (player_info)
        {
            case Player.Info.PLAYER1:
                next = new BuildState(Player.Info.PLAYER2);
                break;
            case Player.Info.PLAYER2:
                next = new TurnState(Player.Info.PLAYER1);
                break;
            default:
                Debug.Log("ERROR: No player found for TurnState");
                next = this;
                break;
        }
    }
    public override void Exit()
    {
        base.Exit();
        Player.G_CURRENT_PLAYER.HoldUnit(null);
        CanvasManager.DisableCanvas(CanvasManager.Menu.STORE);
    }
}

public class TurnState : GameState
{
    public TurnState(Player.Info _info)
    {
        player_info = _info;
    }
    public override void Enter()
    {
        Player.SetPlayer(player_info);
        type = State_Type.TURN;
        switch (player_info)
        {
            case Player.Info.PLAYER1:
                next = new TurnState(Player.Info.PLAYER2);
                break;
            case Player.Info.PLAYER2:
                next = new TurnState(Player.Info.PLAYER1);
                break;
            default:
                Debug.Log("ERROR: No player found for TurnState");
                next = this;
                break;
        }
        SelectionManager.Reselect();
    }
}