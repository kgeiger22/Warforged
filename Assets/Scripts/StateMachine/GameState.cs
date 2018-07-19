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
        MATCH,
    }

    public Player.Type player_info { get; protected set; }
    public State_Type type { get; protected set; }
}

public class LoadState : GameState
{
    public override void Enter()
    {
        type = State_Type.LOAD;
        next = new BuildState(Player.Type.PLAYER1);

    }
}

    public class BuildState : GameState
{
    public BuildState(Player.Type _info)
    {
        player_info = _info;
    }
    public override void Enter()
    {
        PlayerManager.SetPlayerTurn(player_info);
        type = State_Type.BUILD;
        CanvasManager.EnableCanvas(CanvasManager.Menu.UNITPLACE);
        switch (player_info)
        {
            case Player.Type.PLAYER1:
                next = new BuildState(Player.Type.PLAYER2);
                break;
            case Player.Type.PLAYER2:
                next = new MatchStartState();
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
        CanvasManager.DisableCanvas(CanvasManager.Menu.UNITPLACE);
    }
}

public class TurnState : GameState
{
    public TurnState(Player.Type _type)
    {
        player_info = _type;
    }
    public override void Enter()
    {
        PlayerManager.SetPlayerTurn(player_info);
        type = State_Type.TURN;
        switch (player_info)
        {
            case Player.Type.PLAYER1:
                next = new TurnState(Player.Type.PLAYER2);
                break;
            case Player.Type.PLAYER2:
                next = new TurnState(Player.Type.PLAYER1);
                break;
            default:
                Debug.Log("ERROR: No player found for TurnState");
                next = this;
                break;
        }
        EventHandler.StartTurn();
        //put code here that applies after every object has started its turn
        //PlayerManager.GetPlayer(player_info).SelectFirstValidUnit();

    }

    public override void Exit()
    {
        EventHandler.EndTurn();
        //bool player1_round_complete = PlayerManager.GetPlayer(Player.Type.PLAYER1).IsRoundCompleteForThisPlayer();
        //bool player2_round_complete = PlayerManager.GetPlayer(Player.Type.PLAYER2).IsRoundCompleteForThisPlayer();
        //if (player1_round_complete && player2_round_complete)
        //{
        //    next = new RoundState();
        //}
        //else if (player1_round_complete)
        //{
        //    next = new TurnState(Player.Type.PLAYER2);
        //}
        //else if (player2_round_complete)
        //{
        //    next = new TurnState(Player.Type.PLAYER1);
        //}
    }
}

public class RoundState : GameState
{
    public override void Enter()
    {
        if (BaseGame.round_number > 0) EventHandler.EndRound();

        type = State_Type.ROUND;
        next = new TurnState(PlayerManager.GetHighestSPDPlayerType());
        EventHandler.StartRound();
        Global.GAMESTATEFSM.NextState();
        BaseGame.round_number++;
        GameObject.Instantiate(Resources.Load("Prefabs/CanvasNewRound"));
    }
}

public class MatchStartState : GameState
{
    public override void Enter()
    {
        type = State_Type.MATCH;
        next = new RoundState();
        EventHandler.StartMatch();
        Debug.Log("Match Started");
        Global.GAMESTATEFSM.NextState();
    }
}