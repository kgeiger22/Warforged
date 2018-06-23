using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileData : MonoBehaviour {

    public int move_cost { get; protected set; }
    public Tile.Tile_Type type { get; protected set; }
    protected Renderer thisRenderer;

    public virtual void Init()
    {
        Debug.Log("Awake");
        thisRenderer = GetComponent<Renderer>();
    }

    public void Delete()
    {
        Destroy(this);
    }
}

public class FlatlandData : TileData
{
    public override void Init()
    {
        base.Init();
        thisRenderer.material = Resources.Load<Material>("Materials/Flatland");
        move_cost = 1;
        type = Tile.Tile_Type.FLATLAND;
    }
}

public class ForestData : TileData
{
    public override void Init()
    {
        base.Init();
        thisRenderer.material = Resources.Load<Material>("Materials/Forest");
        move_cost = 2;
        type = Tile.Tile_Type.FOREST;
    }
}

public class MountainData : TileData
{
    public override void Init()
    {
        base.Init();
        thisRenderer.material = Resources.Load<Material>("Materials/Mountain");
        move_cost = 3;
        type = Tile.Tile_Type.MOUNTAIN;
    }
}