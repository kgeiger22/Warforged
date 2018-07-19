using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increases defense by 25 until end of round
public class Guard : Ability {

    public Guard(Unit _owner) : base(_owner)
    {
        name = "Guard";
        range = 0;
    }

    public override void Execute(Unit target)
    {
        ApplyEffect(owner, new ModifyDefense(owner, 25, 1));
    }

    protected override bool IsValidTile(Tile tile)
    {
        return true;
    }
}
