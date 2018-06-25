using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM {

    protected FSM_State state;

    public virtual void NextState()
    {
        state.Exit();
        state = state.next;
        state.Enter();
    }

    public virtual void SetState(FSM_State _state)
    {
        if (state != null) state.Exit();
        state = _state;
        state.Enter();
    }
}

public abstract class FSM_State
{
    //public Player.Info player_info { get; protected set; }
    //public State_Type type { get; protected set; }
    public FSM_State next { get; protected set; }

    public virtual void Enter() { }

    public virtual void Exit() { }
}
