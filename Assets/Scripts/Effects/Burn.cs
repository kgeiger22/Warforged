using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deals 10 damage at end of round for 3 turns
public class Burn : Effect {

    public Burn(Unit _owner) : base(_owner)
    {
        duration = 3;
        name = "Burn";
    }

    public override void OnEndOfRound()
    {
        DealAfflictionDamage(10);
    }
}
