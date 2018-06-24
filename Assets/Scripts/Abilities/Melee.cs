using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability {

    public Melee(Unit _owner)
    {
        owner = _owner;
        range = 1;
        amount = 1.0f;
    }

    public override void ApplyEffect(Unit target)
    {
        target.DealDamage(owner.ATK * amount);
    }
}
