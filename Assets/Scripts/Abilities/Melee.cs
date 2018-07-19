using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Melee physical attack 
public class Melee : Attack {

    public Melee(Unit _owner) : base(_owner)
    {
        name = "Melee";
        range = 1;
        attack_type = AttackType.MELEE;
        damage_type = DamageType.PHYSICAL;
    }

    public override void Execute(Unit target)
    {
        DealDamage(target, 1f);
    }
}
