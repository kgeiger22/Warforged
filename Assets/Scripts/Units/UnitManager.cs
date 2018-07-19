using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitManager {

    private static int id_counter = 0;
    public static Dictionary<int, Unit> units { get; private set; }

    public static int AddUnit(Unit _unit)
    {
        if (units == null) units = new Dictionary<int, Unit>();
        _unit.SetID(id_counter);
        units.Add(id_counter, _unit);
        id_counter++;
        return _unit.ID;
    }

    public static Unit GetUnit(int _id)
    {
        return units[_id];
    }

    public static void RemoveUnit(int _id)
    {
        units.Remove(_id);
    }

    public static int RemainingUnitsWithMoves()
    {
        int count = 0;
        foreach (Unit unit in units.Values)
        {
            if (unit.GetState() != UnitState.State_Type.INACTIVE) count++;
        }
        return count;
    }
}
