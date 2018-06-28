using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WarforgedMonoBehaviour {

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
    public Unit held_unit { get; protected set; }
    public List<Unit> units { get; protected set; }

    // Use this for initialization
    protected override void OnGameInit () {
        G_PLAYERS.Add(this);
        info = (Info)G_PLAYERS.Count;
        units = new List<Unit>();
    }

    protected override void OnUpdate()
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
            held_unit.Delete();
        }
        held_unit = _unit;
    }


    public void DropUnit()
    {
        HoldUnit(null);
    }

    public void PlaceUnit()
    {
        held_unit = null;
    }

    public void RemoveUnit(Unit _unit)
    {
        units.Remove(_unit);
    }

    public void AddUnit(Unit _unit)
    {
        units.Add(_unit);
    }

    public List<Unit> GetAllUnits()
    {
        List<Unit> list = new List<Unit>();
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            list.Add(unit.GetComponent<Unit>());
        }
        return list;
    }

    public static bool IsPlayerTurn()
    {
        return (GameStateManager.GetGameState().player_info == G_CURRENT_PLAYER.info);
    }

    public static bool IsPlayerTurn(Info _info)
    {
        return (_info == G_CURRENT_PLAYER.info);
    }

    public static int CalculateSpeed(Info _info)
    {
        switch (_info)
        {
            case Info.PLAYER1:
                return G_PLAYERS[0].CalculateSpeed();
            case Info.PLAYER2:
                return G_PLAYERS[1].CalculateSpeed();
            default:
                return 0;
        }
    }

    public int CalculateSpeed()
    {
        int total_speed = 0;
        foreach(Unit _unit in units)
        {
            total_speed += _unit.SPD;
        }
        return total_speed;
    }
}
