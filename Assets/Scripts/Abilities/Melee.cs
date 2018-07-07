using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability {

    public Melee(Unit _owner)
    {
        name = "Melee";
        owner = _owner;
        range = 1;
    }

    public override void Execute(Unit target)
    {
        DealDamage(target, 1f);
    }
}
