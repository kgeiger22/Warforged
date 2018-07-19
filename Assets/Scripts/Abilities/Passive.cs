using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : Ability {

    protected Effect effect;
    public Passive(Unit _owner, Effect _effect) : base(_owner)
    {
        type = Type.PASSIVE;
        effect = _effect;
    }

    //override to change valid targets
    protected override bool IsValidTile(Tile tile)
    {
        return true;
    }

    public override void Execute(Unit target)
    {
        ApplyEffect(owner, effect);
    }
}
