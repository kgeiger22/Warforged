using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

[ExecuteInEditMode]
public class Tile : MonoBehaviour {


    public enum Tile_Type
    {
        FLATLAND,
        FOREST,
        MOUNTAIN,
    }
    [SerializeField, HideInInspector]
    Tile_Type type;

    [SerializeField, HideInInspector]
    Player.Info owner;

    public List<Tile> adjacency_list { get; protected set; }
    
    public int movement_cost { get; protected set; }
    public int x { get; protected set; }
    public int y { get; protected set; }
    public void SetCoordinates(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public Unit unit { get; protected set; }

    Renderer thisRenderer;
    GameObject highlight;
    Renderer highlight_renderer;
    bool selected = false;
    bool hovered = false;
    [HideInInspector]
    public bool movable;
    [HideInInspector]
    public int distance;
    [HideInInspector]
    public Tile parent;

    public void Init()
    {
        UpdateProperties();
        CreateAdjacencyList();
    }

    protected virtual void Awake()
    {
        Reset();
        adjacency_list = new List<Tile>();
        thisRenderer = GetComponent<Renderer>();
        GameObject go;
        if (!transform.Find("Plane(Clone)"))
        {
            go = Instantiate(Resources.Load<GameObject>("Prefabs/Plane"));
            go.transform.parent = transform;
            go.transform.position += transform.position;
        }
        if (!transform.Find("Highlight(Clone)"))
        {
            go = Instantiate(Resources.Load<GameObject>("Prefabs/Highlight"));
            go.transform.parent = transform;
            go.transform.position += transform.position;
        }
    }

    protected virtual void Start()
    {
        highlight = transform.Find("Highlight(Clone)").gameObject;
        highlight_renderer = highlight.GetComponent<Renderer>();
    }

    protected virtual void Update()
    {
        Material mat;
        if (selected) mat = Resources.Load<Material>("Materials/Selectable");
        else if (hovered) mat = Resources.Load<Material>("Materials/Hoverable");
        else if (movable) mat = Resources.Load<Material>("Materials/Movable");
        else if (owner == Player.Info.PLAYER1) mat = Resources.Load<Material>("Materials/PurpleSelectable");
        else if (owner == Player.Info.PLAYER2) mat = Resources.Load<Material>("Materials/OrangeSelectable");
        else mat = Resources.Load<Material>("Materials/Highlight");
        highlight_renderer.material = mat;
    }

    public void Select()
    {
        selected = true;
        if (unit) unit.Select();
    }

    public void Unselect()
    {
        selected = false;
        if (unit) unit.Unselect();
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
            if (movable && SelectionManager.selected.unit && SelectionManager.selected.unit.BelongsToCurrentPlayer())
            {
                SelectionManager.selected.unit.MoveTo(this);
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

    public void DeleteUnit()
    {
        if (!unit || !ReferenceEquals(Player.G_CURRENT_PLAYER, unit.owner)) return;
        unit.owner.money += Unit.GetCost(unit.type);
        Destroy(unit.gameObject);
        unit = null;
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

    public void SetType(Tile_Type _type)
    {
        //Debug.Log("Setting type " + _type);
        //GetComponent<TileData>().Delete();
        //TileData data = null;
        //switch (_type)
        //{
        //    case Tile_Type.FLATLAND:
        //        data = gameObject.AddComponent<FlatlandData>();
        //        break;
        //    case Tile_Type.FOREST:
        //        data = gameObject.AddComponent<ForestData>();
        //        break;
        //    case Tile_Type.MOUNTAIN:
        //        data = gameObject.AddComponent<MountainData>();
        //        break;
        //}
        //data.Init();
        //type = _type;
    }

    public void UpdateProperties()
    {
        switch (type)
        {
            case Tile_Type.FLATLAND:
                movement_cost = 1;
                thisRenderer.material = Resources.Load<Material>("Materials/Flatland");
                break;
            case Tile_Type.FOREST:
                movement_cost = 2;
                thisRenderer.material = Resources.Load<Material>("Materials/Forest");
                break;
            case Tile_Type.MOUNTAIN:
                movement_cost = 3;
                thisRenderer.material = Resources.Load<Material>("Materials/Mountain");
                break;
        }
    }

    public void CreateAdjacencyList()
    {
        //fill adjacency list
        Tile t;
        //left tile
        t = World.G_WORLD.GetTile(x - 1, y);
        if (t) adjacency_list.Add(t);
        //right tile
        t = World.G_WORLD.GetTile(x + 1, y);
        if (t) adjacency_list.Add(t);
        //down tile
        t = World.G_WORLD.GetTile(x, y - 1);
        if (t) adjacency_list.Add(t);
        //up tile
        t = World.G_WORLD.GetTile(x, y + 1);
        if (t) adjacency_list.Add(t);
    }

    public void Reset()
    {
        movable = false;
        parent = null;
        distance = int.MaxValue;
    }
}

[CustomEditor(typeof(Tile)), CanEditMultipleObjects]
public class TileEditor : Editor
{
    SerializedProperty s_type;
    SerializedProperty s_owner;

    void OnEnable()
    {
        s_type = serializedObject.FindProperty("type");
        s_owner = serializedObject.FindProperty("owner");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(s_type, new GUIContent("Tile Type"));
        EditorGUILayout.PropertyField(s_owner, new GUIContent("Owner"));

        //EditorGUILayout.EnumPopup(new GUIContent("Tile Type"), (Tile.Tile_Type)s_type.enumValueIndex);

        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            foreach (Object tar in targets)
            {
                ((Tile)tar).UpdateProperties();
            }
            
        }
    }
}   