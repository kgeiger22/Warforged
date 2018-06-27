using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public static int cost = 19;

    protected override void OnInstantiate()
    {
        base.OnInstantiate();
        type = Type.ARCHER;
        HP = 50;
        ATK = 85;
        DEF = 8;
        ACC = 0.7f;
        SPD = 5;
    }
}

