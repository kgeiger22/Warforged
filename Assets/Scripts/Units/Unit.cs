using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Draggable))]
public abstract class Unit : WarforgedMonoBehaviour
{

    static float move_speed = 15.0f;

    protected Draggable S_draggable;

    public enum Type
    {
        KNIGHT,
        ARCHER,
        WARHOUND,
    }

    public static int GetCost(Type _type)
    {
        switch (_type)
        {
            case Type.KNIGHT:
                return Knight.cost;
            case Type.ARCHER:
                return Archer.cost;
            case Type.WARHOUND:
                return Warhound.cost;
            default:
                return -1;
        }
    }

    UnitStateFSM fsm;

    [SerializeField]
    public Type type;
    [SerializeField]
    public Material player1_material;
    [SerializeField]
    public Material player2_material;
    
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

    public int health { get; protected set; }
    public int moves_remaining { get; protected set; }


    List<Tile> moveable_tiles = new List<Tile>();
    Stack<Tile> path = new Stack<Tile>();
    protected Vector3 velocity;
    protected Vector3 heading;

    public List<Ability> abilities { get; protected set; }
    public List<Effect> effects { get; protected set; }

    protected override void OnInstantiate()
    {
        fsm = new UnitStateFSM(this);
        S_draggable = GetComponent<Draggable>();
        owner = Player.G_CURRENT_PLAYER;
    }

    protected override void OnPostInstantiate()
    {
        health = HP;
    }

    protected override void OnUpdate()
    {
        if (fsm.GetUnitState().type == UnitState.State_Type.MOVING)
        {
            MoveAlongPath();
        }
    }

    protected override void OnRoundStart()
    {
        moves_remaining = SPD;
    }

    public void Place()
    {
        //apply correct material
        if (owner.info == Player.Info.PLAYER1)
        {
            SetMaterials(player1_material);
        }
        else if (owner.info == Player.Info.PLAYER2)
        {
            SetMaterials(player2_material);
        }
        //move unit to center of tile
        transform.position = SelectionManager.GetSelectedTile().transform.position + new Vector3(0, 0.5f, 0);
        //tell the tile that it owns this unit
        SelectionManager.GetSelectedTile().Place(this);
        //reset player held value
        owner.PlaceUnit();
        //switch unit state upon placement
        fsm.NextState();
        //apply monetary cost to player
        owner.money -= GetCost(type);
        //add unit to player unit pool
        owner.AddUnit(this);
        //generate another draggable, remove this line for single-placement
        UnitFactory.GenerateUnit(type);
        //Update selection
        SelectionManager.Reselect();
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

    public Tile GetCurrentTile()
    {
        return Board.G_BOARD.GetTile(x, y);
    }

    public void FindSelectableTiles()
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
                if (next_tile.IsWalkable() && next_move_cost < next_tile.distance && next_move_cost <= moves_remaining)
                {
                    next_tile.parent = current_tile;
                    next_tile.distance = next_move_cost;
                    process.Enqueue(next_tile);
                }
            }
        }
    }

    public void RemoveSelectableTiles()
    {
        foreach (Tile tile in moveable_tiles)
        {
            tile.Reset();
        }
        moveable_tiles.Clear();
    }

    public void Select()
    {
        CanvasManager.EnableCanvas(CanvasManager.Menu.UNITINFO);
        if (BelongsToCurrentPlayer() && GetCurrentState() == GameState.State_Type.TURN)
        {
            //get unit ready to move
            FindSelectableTiles();
        }
    }

    public void Unselect()
    {
        CanvasManager.DisableCanvas(CanvasManager.Menu.UNITINFO);
        RemoveSelectableTiles();
    }

    public override void Delete()
    {
        Unselect();
        if (owner) owner.RemoveUnit(this);
        Destroy(gameObject);
    }

    public bool BelongsToCurrentPlayer()
    {
        return owner.info == Player.G_CURRENT_PLAYER.info;
    }

    public void MoveTo(Tile tile)
    {
        moves_remaining -= tile.distance;
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
        SelectionManager.Reselect();

        //begin movement
        fsm.SetState(new MovingState(this));
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
            //move to next state
            fsm.NextState();
        }
        else
        {
            Tile t = path.Peek();
            Vector3 target = t.transform.position;

            //calculate units position on top of the target tile
            target.y = transform.position.y;

            if (Vector3.Distance(transform.position, target) >= 0.1f)
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

    public void DisableDraggable()
    {
        S_draggable.enabled = false;
    }

    //adds an ability to a unit's arsenal
    public void AddAbility(Ability _ability)
    {
        abilities.Add(_ability);
    }

    //this is the final calculated value of damage
    public void ReduceHealth(int _damage)
    {
        health -= _damage;
    }

    public void ApplyEffect(Effect _effect)
    {
        _effect.OnApply();
        effects.Add(_effect);
    }
}
