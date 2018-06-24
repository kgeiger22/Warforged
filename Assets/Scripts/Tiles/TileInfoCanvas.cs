using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileInfoCanvas : MonoBehaviour {

    Text tile_type;
    Text tile_cost;

    private void Awake()
    {
        tile_type = transform.Find("InfoHolder").Find("TileType").GetComponent<Text>();
        tile_cost = transform.Find("InfoHolder").Find("TileCost").GetComponent<Text>();
    }

    private void OnEnable()
    {
        UpdateInfo(SelectionManager.selected);
    }

    public void UpdateInfo (Tile tile) {
        if (tile == null)
        {
            tile_type.text = "NULL";
            tile_cost.text = "NULL";
        }
        else
        {
            tile_type.text = "Type: " + tile.GetTileType().ToString();
            tile_cost.text = "Cost: " + tile.movement_cost.ToString();
        }

	}
}
