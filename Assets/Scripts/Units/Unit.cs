using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public abstract class Unit : MonoBehaviour {

    static float move_speed = 15.0f;

    protected Draggable S_draggable;

    public enum Type
    {
        KNIGHT,
        ARCHER,
        HOUND,
    }

    public enum State
    {
        DRAG,
        PLACED,
        MOVING,
    }

    [SerializeField]
    public Type type;
    [SerializeField]
    public Material player1_material;
    [SerializeField]
    public Material player2_material;

    public State current_state { get; protected set; }
    public Player owner { get; protected set; }

    public int HP { get; protected set; }
    public int ATK { get; protected set; }
    public int DEF { get; protected set; }
    public int SPD { get; protected set; }
    public float ACC { get; protected set; }

    public int x { get; protected set; }
    public int y { get; protected set; }
    public void SetCoordinates(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public int moves_remaining { get; protected set; }
    public static int GetCost(Type _type)
    {
        switch (_type)
        {
            case Type.KNIGHT:
                return 25;
            case Type.ARCHER:
                return 21;
            case Type.HOUND:
                return 18;
            default:
                return -1;
        }
    }

    List<Tile> moveable_tiles = new List<Tile>();
    Stack<Tile> path = new Stack<Tile>();
    Vector3 velocity;
    Vector3 heading;

    protected virtual void Awake()
    {
        S_draggable = GetComponent<Draggable>();
        current_state = State.DRAG; //begin in Drag state
        owner = Player.G_CURRENT_PLAYER;
    }

    protected virtual void Update()
    {
        if (current_state == State.MOVING)
        {
            MoveAlongPath();
        }
    }

    protected void SwitchState(State _state)
    {
        switch (_state)
        {
            case State.DRAG: //Drag state used for placing units on the map, used in setup
                current_state = State.DRAG;
                S_draggable.enabled = true;
                break;
            case State.PLACED: //Placed state used for after a unit is dragged onto a tile, used in setup
                S_draggable.enabled = false;
                current_state = State.PLACED;
                break;
            default:
                break;
        }
    }

    public void Place()
    {
        //apply correct material
        if (Player.G_CURRENT_PLAYER.info == Player.Info.PLAYER1)
        {
            SetMaterials(player1_material);
        }
        else if (Player.G_CURRENT_PLAYER.info == Player.Info.PLAYER2)
        {
            SetMaterials(player2_material);
        }
        //move unit to center of tile
        transform.position = SelectionManager.selected.transform.position + new Vector3(0, 0.5f, 0);
        //tell the tile that it owns this unit
        SelectionManager.selected.Place(this);
        //reset player held value
        Player.G_CURRENT_PLAYER.held_unit = null;
        //switch unit state upon placement
        SwitchState(State.PLACED);
        //apply monetary cost to player
        Player.G_CURRENT_PLAYER.money -= GetCost(type);
        //drop the unit from player
        Player.G_CURRENT_PLAYER.DropUnit();
        //generate another draggable, remove this line for single-placement
        UnitFactory.GenerateUnit(type); 
    }

    public void SetMaterials(Material _mat)
    {
        Renderer render;
        Queue<GameObject> queue = new Queue<GameObject>();
        queue.Enqueue(gameObject);
        while(queue.Count > 0)
        {
            GameObject go = queue.Dequeue();
            render = go.GetComponent<Renderer>();
            if (render)
            {
                List<Material> mats = new List<Material>();
                for (int m = 0; m < render.materials.Length; ++m)
                {
                    mats.Add(_mat);
                }
                render.materials = mats.ToArray();
            }
            for (int i = 0; i < go.transform.childCount; i++)
            {
                queue.Enqueue(go.transform.GetChild(i).gameObject);
            }
        }

    }

    private Tile GetCurrentTile()
    {
        return World.G_WORLD.GetTile(x, y);
    }

    private void FindSelectableTiles()
    {
        Queue<Tile> process = new Queue<Tile>();

        Tile current_tile = GetCurrentTile();
        current_tile.distance = 0;
        process.Enqueue(current_tile);
        while (process.Count > 0)
        {
            current_tile = process.Dequeue();
            moveable_tiles.Add(current_tile);
            current_tile.movable = true;

            foreach (Tile next_tile in current_tile.adjacency_list)
            {
                int next_move_cost = next_tile.movement_cost + current_tile.distance;
                if (next_tile.IsWalkable() && next_move_cost < next_tile.distance && next_move_cost <= SPD)
                {
                    next_tile.parent = current_tile;
                    next_tile.distance = next_move_cost;
                    process.Enqueue(next_tile);
                }
            }
        }
    }

    private void RemoveSelectableTiles()
    {
        foreach (Tile tile in moveable_tiles)
        {
            tile.Reset();
        }
        moveable_tiles.Clear();
    }

    public void Select()
    {
        if (current_state == State.PLACED && BelongsToCurrentPlayer() && GameState.G_GAMESTATE.type == GameState.State_Type.TURN)
        {
            //get unit ready to move
            FindSelectableTiles();
        }
    }

    public void Unselect()
    {
        RemoveSelectableTiles();
    }

    public bool BelongsToCurrentPlayer()
    {
        return owner.info == Player.G_CURRENT_PLAYER.info;
    }

    public void MoveTo(Tile tile)
    {
        GetCurrentTile().Unplace();
        tile.Place(this);
        //build path
        path.Clear();
        Tile next_tile = tile;
        while (next_tile.parent != null)
        {
            path.Push(next_tile);
            next_tile = next_tile.parent;
        }
        RemoveSelectableTiles();

        //begin movement
        current_state = State.MOVING;
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * move_speed;
    }

    private void MoveAlongPath()
    {
        //if path is empty, end moving state
        if (path.Count == 0)
        {
            current_state = State.PLACED;
        }
        else
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate units position on top of the target tile
            target.y = transform.position.y;

            if (Vector3.Distance(transform.position, target) >= 0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();

                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //tile center reached
                transform.position = target;
                path.Pop();
            }
        }
    }
}
