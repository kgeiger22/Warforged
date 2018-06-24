using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Unit
{
    public static int cost = 23;

    protected override void Awake()
    {
        base.Awake();
        type = Type.KNIGHT;
        HP = 150;
        ATK = 50;
        DEF = 40;
        ACC = 0.8f;
        SPD = 4;
    }
}
