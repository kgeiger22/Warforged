using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : WarforgedMonoBehaviour {

    public enum Type
    {
        NONE,
        PLAYER1,
        PLAYER2,
    }
    

    public int money { get; protected set; }
    public Type type { get; protected set; }
    public Unit held_unit { get; protected set; }
    public Unit chosen_unit { get; protected set; }

 
    protected override void OnGameInit () {
        if (!Global.PLAYER1)
        {
            type = Type.PLAYER1;
            Global.PLAYER1 = this;
        }
        else if (!Global.PLAYER2)
        {
            type = Type.PLAYER2;
            Global.PLAYER2 = this;
        }
        else
        {
            Debug.Log("Not enough player slots or too many players");
        }
        PopulateArmyList();
        money = 200;
    }


    protected override void OnGamePostInit()
    {
        base.OnGamePostInit();
    }



    public Player GetOtherPlayer()
    {
        switch (type)
        {
            case Type.PLAYER1:
                return Global.PLAYER2;
            case Type.PLAYER2:
                return Global.PLAYER1;
            default:
                return null;
        }
    }


    //public void SelectFirstValidUnit()
    //{
    //    Unit unit;
    //    foreach (int ID in unit_IDs)
    //    {
    //        if (UnitManager.GetUnit(ID).GetState() == UnitState.State_Type.READYTOMOVE)
    //        {
    //            SelectionManager.Select(unit.GetCurrentTile());
    //            Camera.main.GetComponent<MainCamera>().MoveToTile(SelectionManager.GetSelectedTile());
    //            return;
    //        }
    //    }
    //}

    ////will delete the currently held unit if there is one
    //public void HoldUnit(Unit _unit)
    //{
    //    if (held_unit)
    //    {
    //        held_unit.Delete();
    //    }
    //    held_unit = _unit;
    //}


    //public void DeleteHeldUnit()
    //{
    //    HoldUnit(null);
    //}

    //public void DropUnit()
    //{
    //    held_unit = null;
    //}

    // public List<Unit> GetAllUnits()
    // {
    //     List<Unit> list = new List<Unit>();
    //     foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Unit"))
    //     {
    //         list.Add(unit.GetComponent<Unit>());
    //     }
    //     return list;
    // }

    //public static bool IsPlayerTurn()
    //{
    //    return (GameStateManager.GetGameState().player_info == G_CURRENT_PLAYER.info);
    //}



    //public static int CalculateSpeed(Info _info)
    //{
    //    switch (_info)
    //    {
    //        case Info.PLAYER1:
    //            return G_PLAYERS[0].CalculateSpeed();
    //        case Info.PLAYER2:
    //            return G_PLAYERS[1].CalculateSpeed();
    //        default:
    //            return 0;
    //    }
    //}

    // public int CalculateSpeed()
    // {
    //     int total_speed = 0;
    //     foreach(Unit _unit in units)
    //     {
    //         total_speed += _unit.GetModifiedSPD();
    //     }
    //     return total_speed;
    // }


    public void PopulateArmyList()
    {
        UnitFactory.GenerateUnit(Unit.Type.ARCHER, type, false);
        UnitFactory.GenerateUnit(Unit.Type.KNIGHT, type, false);
        UnitFactory.GenerateUnit(Unit.Type.WARHOUND, type, false);
        UnitFactory.GenerateUnit(Unit.Type.WARHOUND, type, false);
    }

    //public void EndTurn()
    //{
    //    chosen_unit = null;
    //    BaseGame.G_GAMESTATEFSM.NextState();
    //}

    //public void SetChosenUnit(Unit _chosen)
    //{
    //    chosen_unit = _chosen;
    //}

    //public bool IsRoundCompleteForThisPlayer()
    //{
    //    foreach(Unit unit in units)
    //    {
    //        if (unit.GetState() != UnitState.State_Type.INACTIVE) return false;
    //    }
    //    return true;
    //}



}
