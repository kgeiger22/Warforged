using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Passive: Heals 10 health at end of round
public class Regeneration : Passive {

    public Regeneration(Unit _owner) : base(_owner, new RegenerateHealth(_owner, 10))
    {
        name = "Regeneration";
    }
}