using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    //Global gamestate
    public static State G_GAMESTATE;

    public enum State_Type
    {
        BEGIN,
        BUILD,
        TURN,
        END,   
    }
    public abstract class State
    {
        public Player.Info player_info { get; protected set; }
        public State_Type type { get; protected set; }
        public State previous { get; protected set; }
        public State next { get; protected set; }
        public virtual void Enter()
        {
            if (previous != null) G_GAMESTATE.Exit();
            previous = G_GAMESTATE;
            G_GAMESTATE = this;
        }
        public virtual void Exit()
        {
        }
    }

    public class BuildState : State
    {
        public BuildState(Player.Info _info)
        {
            player_info = _info;
        }
        public override void Enter()
        {
            base.Enter();
            Player.SetPlayer(player_info);
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
        }
    }

    public class TurnState : State
    {
        public TurnState(Player.Info _info)
        {
            player_info = _info;
        }
        public override void Enter()
        {
            base.Enter();
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

    public static void NextState()
    {
        G_GAMESTATE.Exit();
        G_GAMESTATE.next.Enter();
    }


    private void Start()
    {

        //Set up all data needed to start the game
        Player.G_CURRENT_PLAYER = Player.G_PLAYERS[0];
        if (Player.G_CURRENT_PLAYER == null)
        {
            Debug.Log("NO PLAYERS FOUND");
        }
        G_GAMESTATE = new BuildState(Player.Info.PLAYER1);
        G_GAMESTATE.Enter();


        //Create all adjacency lists for tiles here
        World.G_WORLD.InitalizeTiles();
    }


}
