using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regeneration : Passive {

    public Regeneration(Unit _owner) : base(_owner)
    {
        name = "Regeneration";
    }

    public override void Execute(Unit target)
    {
        ApplyEffect(owner, new RegenerateHealth(target, 10));
    }
}