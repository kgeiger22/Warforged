using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deals 30 damage at end of round for 1 turn
public class Poison : Effect {

	public Poison(Unit _owner) : base(_owner)
    {
        duration = 1;
        name = "Poison";
    }

    public override void OnEndOfRound()
    {
        DealAfflictionDamage(30);
        base.OnEndOfRound();
    }
}
