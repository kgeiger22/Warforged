using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public static int cost = 19;

    public override void InitializeVariables()
    {
        base.InitializeVariables();

        type = Type.ARCHER;
        name = "Hans Fletcherson";
        HP = 50;
        ATK = 85;
        DEF = 8;
        ACC = 70;
        SPD = 5;
        abilities.Add(new RangedShot(this));

    }
}

