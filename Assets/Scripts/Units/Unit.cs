using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : WarforgedMonoBehaviour
{
    float move_speed = 4.0f;

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
    
    public Player.Info owner { get; protected set; }
    public void SetOwner(Player.Info _owner) { owner = _owner; }

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

    
    Queue<Tile> path = new Queue<Tile>();
    public Direction direction;
    protected Vector3 velocity;
    private Tile previous_tile;
    private Tile next_tile;
    private float lerp_time;

    public List<Ability> abilities { get; protected set; }
    public List<Effect> effects { get; protected set; }

    protected int selected_ability_index = 1;
    protected bool IsMoving = false;

    protected override void OnInstantiate()
    {
        fsm = new UnitStateFSM(this);
        //InitializeVariables();
    }
    
    public virtual void InitializeVariables()
    {
        abilities = new List<Ability>();
        abilities.Add(new Wait(this));
        effects = new List<Effect>();
    }

    protected override void OnPostInstantiate()
    {
        health = HP;
    }

    protected override void OnUpdate()
    {
        if (IsMoving)
        {
            MoveAlongPath();
        }
    }

    protected override void OnRoundStart()
    {
        FinishMoveTo();
        moves_remaining = SPD;
        SetState(new ReadyToMoveState(this));
        selected_ability_index = 1;
    }

    protected override void OnRoundEnd()
    {
        for (int i = effects.Count - 1; i >= 0; --i)
        {
            effects[i].OnEndOfRound();
        }
    }

    public void Place(Tile _tile)
    {
        //apply correct material
        if (owner == Player.Info.PLAYER1)
        {
            SetMaterials(player1_material);
        }
        else if (owner == Player.Info.PLAYER2)
        {
            SetMaterials(player2_material);
        }
        //move unit to center of tile
        transform.position = _tile.transform.position + new Vector3(0, 0.5f, 0);
        //tell the tile that it owns this unit
        _tile.Place(this);
        //reset player held value
        GetPlayer(owner).DropUnit();
        //switch unit state upon placement
        SetState(new InactiveState(this));
        //apply monetary cost to player
        GetPlayer(owner).PurchaseUnit(type);
        //add unit to player unit pool
        GetPlayer(owner).AddUnit(this);
        //Update selection
        SelectionManager.Reselect();
        //If the unit was a draggable, disable draggable script
        Draggable draggable = GetComponent<Draggable>();
        if (draggable)
        {
            Destroy(draggable);
        }
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

    public void SetState(UnitState _state)
    {
        if (fsm != null) fsm.SetState(_state);
    }

    public UnitState.State_Type GetState()
    {
        if (fsm != null) return fsm.GetUnitState().type;
        else return UnitState.State_Type.INACTIVE;
    }

    public Tile GetCurrentTile()
    {
        return Board.G_BOARD.GetTile(x, y);
    }

    public Ability GetSelectedAbility()
    {
        return abilities[selected_ability_index];
    }

    public Player GetOwner()
    {
        return GetPlayer(owner);
    }

    protected void HighlightMovableTiles()
    {
        Board.ResetAllTiles();

        Queue<Tile> process = new Queue<Tile>();

        Tile current_tile = GetCurrentTile();
        current_tile.distance = 0;
        process.Enqueue(current_tile);
        while (process.Count > 0)
        {
            current_tile = process.Dequeue();
            current_tile.movable = true;

            foreach (Tile adjacent_tile in current_tile.adjacency_list)
            {
                int next_move_cost = adjacent_tile.movement_cost + current_tile.distance;
                if (adjacent_tile.IsWalkable() && next_move_cost < adjacent_tile.distance && next_move_cost <= moves_remaining)
                {
                    adjacent_tile.parent = current_tile;
                    adjacent_tile.distance = next_move_cost;
                    process.Enqueue(adjacent_tile);
                }
                else
                {
                    adjacent_tile.targettable = true;
                }
            }
        }
    }

    public void Select()
    {
        CanvasManager.EnableCanvas(CanvasManager.Menu.UNITINFO);
        CanvasManager.GetUnitInfoCanvas().UpdateInfo(this);
        if (BelongsToCurrentPlayer())
        {
            //if (GetState() == UnitState.State_Type.READYTOATTACK) SetState(new ReadyToMoveState(this));
            //if (GetState() == UnitState.State_Type.READYTOMOVE) 
            if (GetGameState() == GameState.State_Type.TURN && GetState() != UnitState.State_Type.INACTIVE && (GetOwner().chosen_unit == null || GetOwner().chosen_unit == this))
            {
                if (GetState() == UnitState.State_Type.READYTOATTACK) CanvasManager.EnableCanvas(CanvasManager.Menu.UNITABILITY);
                SelectTiles();
            }
        }
    }

    public void Unselect()
    {
        CanvasManager.DisableCanvas(CanvasManager.Menu.UNITINFO);
        CanvasManager.DisableCanvas(CanvasManager.Menu.UNITABILITY);
        Board.ResetAllTiles();
    }

    public void SelectTiles()
    {
        if (fsm.GetUnitState().type == UnitState.State_Type.READYTOMOVE || fsm.GetUnitState().type == UnitState.State_Type.MOVING)
        {
            //get unit ready to move
            HighlightMovableTiles();
        }
        else if (fsm.GetUnitState().type == UnitState.State_Type.READYTOATTACK)
        {
            //get unit ready to attack
            GetSelectedAbility().HighlightTargettableTiles();
        }
    }

    public override void Delete()
    {
        if (IsSelected()) Unselect();
        GetCurrentTile().Unplace();
        GetPlayer(owner).RemoveUnit(this);
        if (GetGameState() == GameState.State_Type.BUILD)
        {
            CanvasManager.GetCanvas(CanvasManager.Menu.UNITPLACE).GetComponent<UnitPlaceCanvas>().AddUnitPlaceButton(this);
            gameObject.SetActive(false);
            Draggable draggable = GetComponent<Draggable>();
            if (draggable)
            {
                Destroy(draggable);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool BelongsToCurrentPlayer()
    {
        return owner == Player.G_CURRENT_PLAYER.info;
    }

    public void MoveTo(Tile tile)
    {
        if (tile != GetCurrentTile())
        {
            moves_remaining -= tile.distance;
            //build path
            Stack<Tile> stack = new Stack<Tile>();
            Tile t = tile;
            while (t.parent != null)
            {
                stack.Push(t);
                t = t.parent;
            }
            while (stack.Count > 0)
            {
                path.Enqueue(stack.Pop());
            }
            previous_tile = GetCurrentTile();
            next_tile = path.Dequeue();
            //begin movement
            GetCurrentTile().Unplace();
            tile.Place(this);
            IsMoving = true;
        }

        SetState(new ReadyToAttackState(this));
        Board.ResetAllTiles();
        SelectionManager.Reselect();
        GetOwner().SetChosenUnit(this);
        //move camera
        Camera.main.GetComponent<MainCamera>().MoveToTile(tile);
    }

    //only call when a unit is moving, teleports to destination tile
    public void FinishMoveTo()
    {
        if (!IsMoving) return;
        while (path.Count > 0)
        {
            MoveAlongPath();
        }
        fsm.NextState();
    }

    public static Direction GetDirectionBetweenUnits(Unit current, Unit target)
    {
        return Tile.GetDirectionBetweenTiles(current.GetCurrentTile(), target.GetCurrentTile());
    }

    private void MoveAlongPath()
    {
        lerp_time += Time.deltaTime * move_speed;
        Vector3 target_position = Vector3.Lerp(previous_tile.transform.position, next_tile.transform.position, lerp_time);
        transform.position = new Vector3(target_position.x, transform.position.y, target_position.z);
        if (lerp_time >= 1)
        {
            lerp_time = 0;
            if (path.Count == 0)
            {
                IsMoving = false;
                fsm.NextState();
            }
            else
            {
                previous_tile = next_tile;
                next_tile = path.Dequeue();
                FaceDirection(Tile.GetDirectionBetweenTiles(previous_tile, next_tile));
            }
        }
    }

    private void FaceDirection(Direction _direction)
    {
        direction = _direction;
        transform.forward = GetDirectionVector(_direction);
    }
    

    //adds an ability to a unit's arsenal
    public void AddAbility(Ability _ability)
    {
        abilities.Add(_ability);
    }

    //this is the final calculated value of damage
    public void ReceiveDamage(float _damage)
    {
        ActivateReceiveDamageEffects(ref _damage);
        int rounded_damage = Mathf.RoundToInt(_damage);
        health -= rounded_damage;
        DamageText damage_text = Instantiate(Resources.Load<DamageText>("Prefabs/DamageText"));
        damage_text.transform.position = transform.position + new Vector3(0, 5, 0);
        damage_text.SetText("-" + rounded_damage.ToString());
        Debug.Log(name + " took " + rounded_damage + " damage");
        if (health <= 0)
        {
            Debug.Log(name + " fainted");
            Delete();
        }
    }

    protected void ActivateReceiveDamageEffects(ref float _damage)
    {
        foreach (Effect e in effects)
        {
            if (e.type != Effect.Type.RECEIVE_DAMAGE) continue;
            else e.Activate(ref _damage);
        }
    }

    public void ApplyEffect(Effect _effect)
    {
        _effect.OnApply();
        effects.Add(_effect);
    }

    public void ResetWithDraggable()
    {
        gameObject.AddComponent<Draggable>();
    }

    public void ExecuteSelectedAbility(Unit _target)
    {
        FinishMoveTo();
        if (_target && this != _target)
        {
            FaceDirection(GetDirectionBetweenUnits(this, _target));
        }
        Camera.main.GetComponent<MainCamera>().MoveToTile(GetCurrentTile());
        abilities[selected_ability_index].Execute(_target);
        EndTurn();
    }

    public bool IsSelected()
    {
        return this == SelectionManager.GetSelectedUnit();
    }

    public void EndTurn()
    {
        SetState(new InactiveState(this));
        GetOwner().EndTurn();
        //Board.ResetAllTiles();
    }

    public void AddEffect(Effect _effect)
    {
        effects.Add(_effect);
    }

    public void RemoveEffect(Effect _effect)
    {
        effects.Remove(_effect);
    }

    public void SelectAbility(int _ability_index)
    {
        selected_ability_index = _ability_index;
        if (abilities[selected_ability_index].instant_execute)
        {
            ExecuteSelectedAbility(null);
            return;
        }
        SelectTiles();
    }

    public bool IsBehind(Unit _target)
    {
        return false;
    }

}
