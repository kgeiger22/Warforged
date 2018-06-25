using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState : FSM_State {

    public enum State_Type
    {
        DRAG,
        PLACED,
        INACTIVE,
        READYTOMOVE,
        MOVING,
        READYTOATTACK,
        ATTACKING,
    }

    public State_Type type { get; protected set; }
    protected Unit owner;

    public UnitState(Unit _owner) { owner = _owner; }

}

public class DragState : UnitState
{
    public DragState(Unit _owner) : base(_owner) { }

    public override void Enter()
    {
        type = State_Type.DRAG;
        next = new InactiveState(owner);
    }

    public override void Exit()
    {
        owner.DisableDraggable();
    }
}

public class ReadyToMoveState : UnitState
{
    public ReadyToMoveState(Unit _owner) : base(_owner) { }

    public override void Enter()
    {
        type = State_Type.READYTOMOVE;
        next = new MovingState(owner);
    }
}

public class ReadyToAttackState : UnitState
{
    public ReadyToAttackState(Unit _owner) : base(_owner) { }

    public override void Enter()
    {
        type = State_Type.READYTOATTACK;
        next = new AttackingState(owner);
    }
}

public class InactiveState : UnitState
{
    public InactiveState(Unit _owner) : base(_owner) { }

    public override void Enter()
    {
        type = State_Type.INACTIVE;
        next = new ReadyToMoveState(owner);
    }
}

public class MovingState : UnitState
{
    public MovingState(Unit _owner) : base(_owner) { }

    public override void Enter()
    {
        type = State_Type.MOVING;
        next = new ReadyToMoveState(owner);
    }
}

public class AttackingState : UnitState
{
    public AttackingState(Unit _owner) : base(_owner) { }

    public override void Enter()
    {
        type = State_Type.ATTACKING;
        next = new ReadyToAttackState(owner);
    }
}



