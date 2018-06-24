using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhound : Unit
{
    public static int cost = 17;

    protected override void Awake()
    {
        base.Awake();
        type = Type.WARHOUND;
        HP = 110;
        ATK = 50;
        DEF = 12;
        ACC = 0.7f;
        SPD = 6;
    }
}
