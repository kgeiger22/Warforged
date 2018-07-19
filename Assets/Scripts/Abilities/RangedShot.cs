using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ranged physical attack that can hit targets 3 tiles away
public class RangedShot : Attack {

    public RangedShot(Unit _owner) : base(_owner)
    {
        name = "Ranged Shot";
        range = 3;
        attack_type = AttackType.RANGED;
        damage_type = DamageType.PHYSICAL;
    }

    public override void Execute(Unit target)
    {
        DealDamage(target, 1f);
    }
}
