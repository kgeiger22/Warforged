using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : Tile {

    protected override void OnGameInit()
    {
        base.OnGameInit();
        movement_cost = 2;
        type = Tile_Type.FOREST;
    }
}
