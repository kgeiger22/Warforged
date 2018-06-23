using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{

    protected override void Awake()
    {
        base.Awake();
        type = Type.ARCHER;
        HP = 50;
        ATK = 80;
        DEF = 8;
        SPD = 5;
        ACC = 0.7f;
    }
}

