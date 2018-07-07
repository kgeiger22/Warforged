using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory {

    public static Unit GenerateUnit(Unit.Type _type, Player.Info _info)
    {
        Unit unit;
        switch (_type)
        {
            case Unit.Type.KNIGHT:
                unit = GameObject.Instantiate(Resources.Load<Unit>("Prefabs/Knight"));
                break;
            case Unit.Type.ARCHER:
                unit = GameObject.Instantiate(Resources.Load<Unit>("Prefabs/Archer"));
                break;
            case Unit.Type.WARHOUND:
                unit = GameObject.Instantiate(Resources.Load<Unit>("Prefabs/Warhound"));
                break;
            default:
                Debug.Log("ERROR: Unit type not found");
                return null;
        }

        switch (_info)
        {
            case Player.Info.PLAYER1:
                unit.transform.Rotate(0, 90, 0);
                break;
            case Player.Info.PLAYER2:
                unit.transform.Rotate(0, -90, 0);
                break;
            default:
                Debug.Log("ERROR: Unit owner unassigned at generation");
                break;
        }
        unit.SetOwner(_info);
        unit.InitializeVariables();

        return unit;
    }

    //public static Unit GenerateDraggableUnit(Unit.Type _type)
    //{
    //    //draggable units always belong to the current player
    //    Unit unit = GenerateUnit(_type, Player.G_CURRENT_PLAYER.info);
    //    unit.gameObject.AddComponent<Draggable>();
    //    return unit;
    //}
    //
    //public static Unit GenerateDraggableUnit(Unit.Type _type, Player.Info _info)
    //{
    //    //draggable units always belong to player
    //    Unit unit = GenerateUnit(_type, _info);
    //    unit.gameObject.AddComponent<Draggable>();
    //    return unit;
    //}
}
