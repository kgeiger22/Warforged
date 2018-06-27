using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mountain : Tile {

    protected override void OnGameInit()
    {
        base.OnGameInit();
        movement_cost = 3;
        type = Tile_Type.MOUNTAIN;
    }
}
