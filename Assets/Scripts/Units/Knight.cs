using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Unit
{

    protected override void Awake()
    {
        base.Awake();
        type = Type.KNIGHT;
        HP = 190;
        ATK = 100;
        DEF = 60;
        SPD = 4;
        ACC = 0.8f;
    }
}
