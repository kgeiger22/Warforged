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

    public int money { get; protected set; }
    public Info info { get; protected set; }
    public Unit held_unit { get; protected set; }
    public Unit chosen_unit { get; protected set; }
    public List<Unit> army { get; protected set; }
    public List<Unit> units { get; protected set; }

    public static new Player GetPlayer(Info _info)
    {
        switch (_info)
        {
            case Info.PLAYER1:
                return G_PLAYERS[0];
            case Info.PLAYER2:
                return G_PLAYERS[1];
            default:
                return null;
        }
    }

    // Use this for initialization
    protected override void OnGameInit () {
        G_PLAYERS.Add(this);
        info = (Info)G_PLAYERS.Count;
        //populate units list
        units = new List<Unit>();
        PopulateArmyList();
        money = 200;
    }


    protected override void OnGamePostInit()
    {
        base.OnGamePostInit();
    }

    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(1)) SelectionManager.Unselect();
    }

    public void SelectFirstValidUnit()
    {
        foreach (Unit unit in units)
        {
            if (unit.GetState() == UnitState.State_Type.READYTOMOVE)
            {
                SelectionManager.Select(unit.GetCurrentTile());
                Camera.main.GetComponent<MainCamera>().MoveToTile(SelectionManager.GetSelectedTile());
                return;
            }
        }
    }


    public static void SwitchPlayers()
    {
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

    //will delete the currently held unit if there is one
    public void HoldUnit(Unit _unit)
    {
        if (held_unit)
        {
            held_unit.Delete();
        }
        held_unit = _unit;
    }


    public void DeleteHeldUnit()
    {
        HoldUnit(null);
    }

    public void DropUnit()
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

    public bool HasFunds(Unit.Type _type)
    {
        return money >= Unit.GetCost(_type);
    }

    public void PurchaseUnit(Unit.Type _type)
    {
        money -= Unit.GetCost(_type);
        if (money < 0) Debug.Log("ERROR: Player has negative funds");
    }

    public void RefundUnit(Unit.Type _type)
    {
        money += Unit.GetCost(_type);
    }

    public void PopulateArmyList()
    {
        army = new List<Unit>
        {
            UnitFactory.GenerateUnit(Unit.Type.KNIGHT, info),
            UnitFactory.GenerateUnit(Unit.Type.KNIGHT, info),
            UnitFactory.GenerateUnit(Unit.Type.KNIGHT, info),
            UnitFactory.GenerateUnit(Unit.Type.ARCHER, info),
            UnitFactory.GenerateUnit(Unit.Type.ARCHER, info),
            UnitFactory.GenerateUnit(Unit.Type.WARHOUND, info)
        };
        foreach (Unit unit in army)
        {
            unit.gameObject.SetActive(false);
        }
    }

    public void EndTurn()
    {
        chosen_unit = null;
        BaseGame.G_GAMESTATEFSM.NextState();
    }

    public void SetChosenUnit(Unit _chosen)
    {
        chosen_unit = _chosen;
    }

    public bool IsRoundCompleteForThisPlayer()
    {
        foreach(Unit unit in units)
        {
            if (unit.GetState() != UnitState.State_Type.INACTIVE) return false;
        }
        return true;
    }


    public Player GetOtherPlayer()
    {
        switch (info)
        {
            case Info.PLAYER1:
                return GetPlayer(Info.PLAYER2);
            case Info.PLAYER2:
                return GetPlayer(Info.PLAYER1);
            default:
                return null;
        }
    }
}
