using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Unit
{
    public static int cost = 23;

    public override void InitializeVariables()
    {
        base.InitializeVariables();
        type = Type.KNIGHT;
        name = "Diane the Dim";
        HP = 150;
        ATK = 50;
        DEF = 40;
        ACC = 80;
        SPD = 4;
        abilities.Add(new Melee(this));
        abilities.Add(new Guard(this));
        abilities.Add(new Regeneration(this));
    }
}
