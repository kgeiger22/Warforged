using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

[RequireComponent(typeof(TileEditor))]
public abstract class Tile : WarforgedMonoBehaviour
{
    public static readonly float width = 4.0f;

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
    public Player.Type owner { get; protected set; }
    public void SetOwner(Player.Type _info)
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

    void InitializeMaterials()
    {
        if (material_selectable == null)
        {
            material_selectable = Resources.Load<Material>("Materials/Selectable");
            materrial_hoverable = Resources.Load<Material>("Materials/Hoverable");
            materrial_movable = Resources.Load<Material>("Materials/Movable");
            material_valid = Resources.Load<Material>("Materials/Valid");
            material_targettable = Resources.Load<Material>("Materials/Targettable");
            material_player1 = Resources.Load<Material>("Materials/PurpleSelectable");
            material_player2 = Resources.Load<Material>("Materials/OrangeSelectable");
            material_highlight = Resources.Load<Material>("Materials/Highlight");
        }
    }
    static Material material_selectable;
    static Material materrial_hoverable;
    static Material materrial_movable;
    static Material material_valid;
    static Material material_targettable;
    static Material material_player1;
    static Material material_player2;
    static Material material_highlight;


    protected override void OnGameInit()
    {
        InitializeMaterials();
        adjacency_list = new List<Tile>();
        CreateAdjacencyList();
        Reset();
        owner = GetComponent<TileEditor>().editor_owner;
        GetComponent<TileEditor>().enabled = false;
    }

    protected override void OnUpdate()
    {
        Material mat;
        if (valid) mat = material_valid;
        else if (selected) mat = material_selectable;
        else if (hovered) mat = materrial_hoverable;
        else if (movable) mat = materrial_movable;
        else if (targettable) mat = material_targettable;
        else if (GetGameState() == GameState.State_Type.BUILD && owner == Player.Type.PLAYER1) mat = material_player1;
        else if (GetGameState() == GameState.State_Type.BUILD && owner == Player.Type.PLAYER2) mat = material_player2;
        else mat = material_highlight;
        GetHighlightRenderer().material = mat;
    }

    public void OnSelect()
    {
        selected = true;
        if (unit) unit.OnSelect();
        CanvasManager.EnableCanvas(CanvasManager.Menu.TILEINFO);
    }

    public void OnUnselect()
    {
        selected = false;
        if (unit) unit.OnUnselect();
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
            Unit selected_unit = SelectionManager.GetSelectedUnit();
            if (movable && selected_unit && selected_unit.BelongsToCurrentPlayer())
            {
                SelectionManager.GetSelectedUnit().MoveTo(this);
            }
            if (valid && selected_unit && selected_unit.BelongsToCurrentPlayer())
            {
                selected_unit.ExecuteSelectedAbility(unit);
                return;
            }
            //put placement code here
            SelectionManager.Select(this);
        }
    }

    public void OnMouseEnter()
    {
        SelectionManager.Hover(this);

    }

    public void OnMouseExit()
    {
        SelectionManager.Unhover();
    }

    public bool BelongsToCurrentPlayer()
    {
        return PlayerManager.CurrentPlayer == owner;
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


    public void ChangeTile(Tile_Type _type, Player.Type _info)
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
        if (owner == Player.Type.PLAYER1) GetHighlightRenderer().material = Resources.Load<Material>("Materials/PurpleSelectable");
        else if (owner == Player.Type.PLAYER2) GetHighlightRenderer().material = Resources.Load<Material>("Materials/OrangeSelectable");
    }

    public static Direction GetDirectionBetweenTiles(Tile current, Tile target)
    {
        float dx = target.x - current.x;
        float dz = target.y - current.y;
        if (Mathf.Abs(dx) >= Mathf.Abs(dz))
        {
            //left or right
            return (dx > 0) ? Direction.RIGHT : Direction.LEFT;
        }
        else //up or down
        {
            return (dz > 0) ? Direction.UP : Direction.DOWN;
        }
    }
}

