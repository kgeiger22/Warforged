using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedShot : Ability {


    public RangedShot(Unit _owner)
    {
        name = "Ranged Shot";
        owner = _owner;
        range = 2;
    }

    public override void Execute(Unit target)
    {
        DealDamage(target, 1f);
    }
}
