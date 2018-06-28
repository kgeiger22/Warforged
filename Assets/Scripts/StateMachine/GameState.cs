using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : FSM_State {

    public enum State_Type
    {
        LOAD,
        BUILD,
        TURN,
        END,   
        ROUND,
    }

    public Player.Info player_info { get; protected set; }
    public State_Type type { get; protected set; }
}

public class LoadState : GameState
{
    public override void Enter()
    {
        type = State_Type.LOAD;
        next = new BuildState(Player.Info.PLAYER1);

    }
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
                next = new RoundState();
                break;
            default:
                Debug.Log("ERROR: No player found for TurnState");
                next = this;
                break;
        }
        SelectionManager.Unselect();
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
        SelectionManager.Unselect();
        EventHandler.StartTurn();
    }

    public override void Exit()
    {
        EventHandler.EndTurn();
    }
}

public class RoundState : GameState
{
    public override void Enter()
    {
        type = State_Type.ROUND;
        int speed_player1 = Player.CalculateSpeed(Player.Info.PLAYER1);
        int speed_player2 = Player.CalculateSpeed(Player.Info.PLAYER2);
        if (speed_player1 > speed_player2)
        {
            next = new TurnState(Player.Info.PLAYER1);
        }
        else if (speed_player2 > speed_player1)
        {
            next = new TurnState(Player.Info.PLAYER2);
        }
        else
        {
            //decide randomly
            float rng = Random.value;
            if (rng < 0.5f) next = new TurnState(Player.Info.PLAYER1);
            else next = new TurnState(Player.Info.PLAYER2);
        }
        EventHandler.StartRound();
        BaseGame.G_GAMESTATEFSM.NextState();
    }
}