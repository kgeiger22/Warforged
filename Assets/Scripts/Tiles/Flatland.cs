using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Flatland : Tile {

    protected override void OnGameInit()
    {
        base.OnGameInit();
        movement_cost = 1;
        type = Tile_Type.FLATLAND;
    }
}