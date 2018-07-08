using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateHealth : Effect {

    int amount;

    public RegenerateHealth(Unit _owner, int _amount) : base(_owner)
    {
        amount = _amount;
        duration = -1;
        type = Type.END_OF_ROUND;
        name = "Regenerate";
    }

    public override void Activate()
    {
        owner.ReceiveHealing(amount);
    }
}
