using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Passive : Ability {

    public Passive(Unit _owner) : base(_owner)
    {
        type = Type.PASSIVE;
    }

    //override to change valid targets
    protected override bool IsValidTile(Tile tile)
    {
        return true;
    }
}
