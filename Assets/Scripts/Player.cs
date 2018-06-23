using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum Info
    {
        NONE,
        PLAYER1,
        PLAYER2,
    }

    public static List<Player> G_PLAYERS = new List<Player>();
    public static Player G_CURRENT_PLAYER;

    public int money;
    public Info info { get; protected set; }
    public Unit held_unit;

    // Use this for initialization
    private void Awake () {
        G_PLAYERS.Add(this);
        info = (Info)G_PLAYERS.Count;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) SelectionManager.Unselect();
    }

    public static void SwitchPlayers()
    {
        G_CURRENT_PLAYER.DropUnit();
        switch (G_CURRENT_PLAYER.info)
        {
            case Info.PLAYER1:
                G_CURRENT_PLAYER = G_PLAYERS[1];
                break;
            default:
                G_CURRENT_PLAYER = G_PLAYERS[0];
                break;
        }
        SelectionManager.Unselect();
    }

    public static void SetPlayer(Info _info)
    {
        switch (_info)
        {
            case Info.NONE:
                break;
            case Info.PLAYER1:
                G_CURRENT_PLAYER = G_PLAYERS[0];
                break;
            case Info.PLAYER2:
                G_CURRENT_PLAYER = G_PLAYERS[1];
                break;
            default:
                break;
        }
    }

    public void HoldUnit(Unit _unit)
    {
        if (held_unit)
        {
            Destroy(held_unit.gameObject);
        }
        held_unit = _unit;
    }

    public void DropUnit()
    {
        HoldUnit(null);
    }

    public static bool IsPlayerTurn()
    {
        return (GameState.G_GAMESTATE.player_info == G_CURRENT_PLAYER.info);
    }

    public static bool IsPlayerTurn(Info _info)
    {
        return (_info == G_CURRENT_PLAYER.info);
    }
}
