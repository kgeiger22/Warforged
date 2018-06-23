using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory {

    public static void GenerateUnit(Unit.Type _type, Tile _tile)
    {
        if (Player.G_CURRENT_PLAYER.money < Unit.GetCost(_type)) return; //only generate unit if player has sufficient funds

        Unit unit;
        switch (_type)
        {
            case Unit.Type.KNIGHT:
                unit = GameObject.Instantiate(Resources.Load<Unit>("Prefabs/Knight"));
                break;
            case Unit.Type.ARCHER:
                unit = GameObject.Instantiate(Resources.Load<Unit>("Prefabs/Archer"));
                break;
            case Unit.Type.HOUND:
                unit = GameObject.Instantiate(Resources.Load<Unit>("Prefabs/Warhound"));
                break;
            default:
                return;
        }
        //if a tile is passed in, place the unit immediately
        if (_tile)
        {
            SelectionManager.Select(_tile);
            unit.Place();
        }

        //turn the character the correct direction
        if (Player.G_CURRENT_PLAYER.info == Player.Info.PLAYER1) unit.transform.Rotate(0, 90, 0);
        else if (Player.G_CURRENT_PLAYER.info == Player.Info.PLAYER2) unit.transform.Rotate(0, -90, 0);

        //replace the currently held unit
        Player.G_CURRENT_PLAYER.HoldUnit(unit);
    }

    public static void GenerateUnit(Unit.Type _type)
    {
        GenerateUnit(_type, null);
    }
}
