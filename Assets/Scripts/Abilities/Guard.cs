﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Ability {

    public Guard(Unit _owner)
    {
        name = "Guard";
        owner = _owner;
        range = 0;
    }

    public override void Execute(Unit target)
    {
        ApplyEffect(owner, new AmplifyDamageReceived(owner, 0.5f));
    }

    protected override bool IsValidTile(Tile tile)
    {
        return true;
    }
}