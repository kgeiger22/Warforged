using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory {

    public static int GenerateUnit(Unit.Type _type, Player.Type _info, bool _active = true)
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
                return -1;
        }

        switch (_info)
        {
            case Player.Type.PLAYER1:
                unit.FaceDirection(WarforgedMonoBehaviour.Direction.RIGHT);
                break;
            case Player.Type.PLAYER2:
                unit.FaceDirection(WarforgedMonoBehaviour.Direction.LEFT);
                break;
            default:
                Debug.Log("ERROR: Unit owner unassigned at generation");
                break;
        }
        unit.SetOwner(_info);
        unit.InitializeVariables();
        unit.gameObject.SetActive(_active);
        return UnitManager.AddUnit(unit);
    }

    //public static Unit GenerateDraggableUnit(Unit.Type _type)
    //{
    //    //draggable units always belong to the current player
    //    Unit unit = GenerateUnit(_type, PlayerManager.CurrentPlayer.info);
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
