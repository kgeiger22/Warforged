using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Passive: Heals X health at end of each round
public class RegenerateHealth : Effect {

    int amount;

    public RegenerateHealth(Unit _owner, int _amount) : base(_owner)
    {
        amount = _amount;
        duration = -1;
        name = "Regenerate";
    }

    public override void OnEndOfRound()
    {
        owner.ReceiveHealing(amount);
        base.OnEndOfRound();
    }
}
