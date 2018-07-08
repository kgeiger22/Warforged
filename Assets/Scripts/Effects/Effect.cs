using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect {

    public int duration = -1;
    public Type type;
    protected Unit owner;
    public string name;

    public enum Type
    {
        START_OF_ROUND,
        END_OF_ROUND,
        DEAL_DAMAGE,
        RECEIVE_DAMAGE,
    }
    

    public Effect(Unit _owner)
    {
        owner = _owner;
    }

    public virtual void Delete()
    {
        Debug.Log("Effect " + name + " removed");
        owner.RemoveEffect(this);
    }

    public virtual void OnApply() { }

    public virtual void OnEndOfRound() {
        duration--;
        if (duration == 0) Delete();
    }

    public virtual void Activate() { }
    public virtual void Activate(ref float val) { }

}
