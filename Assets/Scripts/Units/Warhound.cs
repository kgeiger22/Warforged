using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhound : Unit
{
    public static int cost = 17;

    public override void InitializeVariables()
    {
        base.InitializeVariables();

        type = Type.WARHOUND;
        name = "Snuffles the Good Dog";
        HP = 110;
        ATK = 50;
        DEF = 12;
        ACC = 0.7f;
        SPD = 6;
        abilities.Add(new Melee(this));

    }
}
