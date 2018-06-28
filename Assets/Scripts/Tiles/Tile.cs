using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

[RequireComponent(typeof(TileEditor))]
public abstract class Tile : WarforgedMonoBehaviour
{
    public enum Tile_Type
    {
        FLATLAND,
        FOREST,
        MOUNTAIN,
    }

    [SerializeField, HideInInspector]
    public Tile_Type type { get; protected set; }
    public int movement_cost { get; protected set; }

    public Unit unit { get; protected set; }
    public Player.Info owner { get; protected set; }
    public void SetOwner(Player.Info _info)
    {
        owner = _info;
    }

    public int x { get; protected set; }
    public int y { get; protected set; }
    public void SetCoordinates(int _x, int _y)
    {
        x = _x;
        y = _y;
    }


    public List<Tile> adjacency_list { get; protected set; }
    bool selected = false;
    bool hovered = false;
    [HideInInspector]
    public bool movable;
    [HideInInspector]
    public bool targettable;
    [HideInInspector]
    public bool valid;
    [HideInInspector]
    public int distance;
    [HideInInspector]
    public Tile parent;

    protected override void OnGameInit()
    {
        adjacency_list = new List<Tile>();
        CreateAdjacencyList();
        Reset();
        owner = GetComponent<TileEditor>().editor_owner;
        GetComponent<TileEditor>().enabled = false;
    }

    protected override void OnUpdate()
    {
        Material mat;
        if (selected) mat = Resources.Load<Material>("Materials/Selectable");
        else if (hovered) mat = Resources.Load<Material>("Materials/Hoverable");
        else if (movable) mat = Resources.Load<Material>("Materials/Movable");
        else if (valid) mat = Resources.Load<Material>("Materials/Valid");
        else if (targettable) mat = Resources.Load<Material>("Materials/Targettable");
        else if (GetCurrentState() == GameState.State_Type.BUILD && owner == Player.Info.PLAYER1) mat = Resources.Load<Material>("Materials/PurpleSelectable");
        else if (GetCurrentState() == GameState.State_Type.BUILD && owner == Player.Info.PLAYER2) mat = Resources.Load<Material>("Materials/OrangeSelectable");
        else mat = Resources.Load<Material>("Materials/Highlight");
        GetHighlightRenderer().material = mat;
    }

    public void Select()
    {
        selected = true;
        if (unit) unit.Select();
        CanvasManager.EnableCanvas(CanvasManager.Menu.TILEINFO);
    }

    public void Unselect()
    {
        selected = false;
        if (unit) unit.Unselect();
        CanvasManager.DisableCanvas(CanvasManager.Menu.TILEINFO);
    }

    public void Hover()
    {
        hovered = true;
    }

    public void Unhover()
    {
        hovered = false;
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //check if moving to tile
            if (movable && SelectionManager.GetSelectedUnit() && SelectionManager.GetSelectedUnit().BelongsToCurrentPlayer())
            {
                SelectionManager.GetSelectedUnit().MoveTo(this);
            }

            SelectionManager.Select(this);
            if (Player.G_CURRENT_PLAYER.held_unit && Player.G_CURRENT_PLAYER.held_unit.GetComponent<Draggable>().IsPlaceable()) Player.G_CURRENT_PLAYER.held_unit.Place();
        }
    }

    public void OnMouseEnter()
    {
        HoverManager.Hover(this);

    }

    public void OnMouseExit()
    {
        HoverManager.Unhover();
    }

    public bool BelongsToCurrentPlayer()
    {
        return Player.G_CURRENT_PLAYER.info == owner;
    }

    public bool IsWalkable()
    {
        return unit == null;
    }

    public void Place(Unit _unit)
    {
        unit = _unit;
        unit.SetCoordinates(x, y);
    }

    public void Unplace()
    {
        unit = null;
    }


    public void ChangeTile(Tile_Type _type, Player.Info _info)
    {
        Tile new_tile = null;
        switch (_type)
        {
            case Tile_Type.FLATLAND:
                new_tile = Instantiate(Resources.Load<Tile>("Prefabs/Flatland"));
                break;
            case Tile_Type.FOREST:
                new_tile = Instantiate(Resources.Load<Tile>("Prefabs/Forest"));
                break;
            case Tile_Type.MOUNTAIN:
                new_tile = Instantiate(Resources.Load<Tile>("Prefabs/Mountain"));
                break;
            default:
                break;
        }
        if (!new_tile) return;
        new_tile.name = name;
        new_tile.transform.parent = transform.parent;
        new_tile.transform.position = transform.position;
        new_tile.transform.SetSiblingIndex(transform.GetSiblingIndex());
        new_tile.SetOwner(_info);
        new_tile.SetPlayerHighlight();
        new_tile.GetComponent<TileEditor>().editor_owner = _info;
        new_tile.GetComponent<TileEditor>().editor_type = _type;

    }

    public void CreateAdjacencyList()
    {
        //fill adjacency list
        Tile t;
        //left tile
        t = Board.G_BOARD.GetTile(x - 1, y);
        if (t) adjacency_list.Add(t);
        //right tile
        t = Board.G_BOARD.GetTile(x + 1, y);
        if (t) adjacency_list.Add(t);
        //down tile
        t = Board.G_BOARD.GetTile(x, y - 1);
        if (t) adjacency_list.Add(t);
        //up tile
        t = Board.G_BOARD.GetTile(x, y + 1);
        if (t) adjacency_list.Add(t);
    }

    public void Reset()
    {
        movable = false;
        valid = false;
        targettable = false;
        parent = null;
        distance = int.MaxValue;
    }

    public Renderer GetRenderer()
    {
        return GetComponent<Renderer>();
    }

    public Renderer GetHighlightRenderer()
    {
        Transform h = transform.Find("Highlight");
        if (h) return h.GetComponent<Renderer>();
        else return null;
    }

    public void SetPlayerHighlight()
    {
        if (owner == Player.Info.PLAYER1) GetHighlightRenderer().material = Resources.Load<Material>("Materials/PurpleSelectable");
        else if (owner == Player.Info.PLAYER2) GetHighlightRenderer().material = Resources.Load<Material>("Materials/OrangeSelectable");
    }
}

