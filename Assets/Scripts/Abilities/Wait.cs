using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : Ability {

    public Wait(Unit _owner) : base(_owner)
    {
        name = "Wait";
        range = 0;
        instant_execute = true;
    }

    public override void Execute(Unit target)
    {
    }

    protected override bool IsValidTile(Tile tile)
    {
        return true;
    }
}
