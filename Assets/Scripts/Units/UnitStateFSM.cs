using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateFSM : FSM {

    Unit owner;

    public UnitStateFSM(Unit _owner)
    {
        owner = _owner;
        SetState(new DragState(_owner));
    }

    public UnitState GetUnitState()
    {
        return state as UnitState;
    }

    private void Start()
    {
        state = new DragState(owner);
        state.Enter();
    }
}
