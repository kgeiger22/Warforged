using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warhound : Unit
{

    protected override void Awake()
    {
        base.Awake();
        type = Type.HOUND;
        HP = 150;
        ATK = 50;
        DEF = 12;
        SPD = 6;
        ACC = 0.7f;
    }
}
