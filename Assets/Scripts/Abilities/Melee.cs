using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Ability {

    public Melee(Unit _owner)
    {
        owner = _owner;
        range = 1;
    }
}
