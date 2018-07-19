using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect {

    public int duration = -1;
    protected Unit owner;
    public string name;
    

    public Effect(Unit _owner)
    {
        owner = _owner;
    }

    public virtual void Delete()
    {
        owner.RemoveEffect(this);
    }

    public void DealAfflictionDamage(int _amount)
    {
        owner.ReceiveDamage(_amount);
    }

    public virtual void OnEndOfRound()
    {
        duration--;
        if (duration == 0) Delete();
    }

    public virtual void OnApply() { }

    public virtual void OnStartOfRound() { }
}
