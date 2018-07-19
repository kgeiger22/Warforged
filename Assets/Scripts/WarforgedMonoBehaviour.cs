using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarforgedMonoBehaviour : MonoBehaviour {

    public static readonly Vector3 up = new Vector3(0, 0, 1);
    public static readonly Vector3 down = new Vector3(0, 0, -1);
    public static readonly Vector3 left = new Vector3(-1, 0, 0);
    public static readonly Vector3 right = new Vector3(1, 0, 0);

    public enum Direction
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public static Vector3 GetDirectionVector(Direction _direction)
    {
        switch (_direction)
        {
            case Direction.UP:
                return up;
            case Direction.DOWN:
                return down;
            case Direction.LEFT:
                return left;
            case Direction.RIGHT:
                return right;
            default:
                return Vector3.zero;
        }
    }

    public static bool AreOppositeDirections(Direction d1, Direction d2)
    {
        return GetDirectionVector(d1) + GetDirectionVector(d2) == Vector3.zero;
    }

    public static bool AreSameDirection(Direction d1, Direction d2)
    {
        return d1 == d2;
    }

    protected void Awake()
    {
        AddListeners();
    }

    protected void AddListeners()
    {
        EventHandler._OnGamePreInit  += OnGamePreInit;
        EventHandler._OnGameInit     += OnGameInit;
        EventHandler._OnGamePostInit += OnGamePostInit;
        EventHandler._OnGameStart    += OnGameStart;
        EventHandler._OnTurnStart    += OnTurnStart;
        EventHandler._OnTurnEnd      += OnTurnEnd;
        EventHandler._OnRoundStart   += OnRoundStart;
        EventHandler._OnRoundEnd     += OnRoundEnd;
        EventHandler._OnMatchStart   += OnMatchStart;
        EventHandler._OnMatchEnd     += OnMatchEnd;
    }

    protected void RemoveListeners()
    {
        EventHandler._OnGamePreInit  -= OnGamePreInit;
        EventHandler._OnGameInit     -= OnGameInit;
        EventHandler._OnGamePostInit -= OnGamePostInit;
        EventHandler._OnGameStart    -= OnGameStart;
        EventHandler._OnTurnStart    -= OnTurnStart;
        EventHandler._OnTurnEnd      -= OnTurnEnd;
        EventHandler._OnRoundStart   -= OnRoundStart;
        EventHandler._OnRoundEnd     -= OnRoundEnd;
        EventHandler._OnMatchStart   -= OnMatchStart;
        EventHandler._OnMatchEnd     -= OnMatchEnd;
    }


    protected void Start()
    {
        OnInstantiate();
        OnPostInstantiate();
    }

    protected void Update()
    {
        if (GameStateFSM.GetGameState().type != GameState.State_Type.LOAD)
        {
            OnUpdate();
        }
    }

    protected virtual void OnGamePreInit() { }
    protected virtual void OnGameInit() { }
    protected virtual void OnGamePostInit() { }
    protected virtual void OnGameStart() { }
    protected virtual void OnTurnStart() { }
    protected virtual void OnTurnEnd() { }
    protected virtual void OnRoundStart() { }
    protected virtual void OnRoundEnd() { }
    protected virtual void OnMatchStart() { }
    protected virtual void OnMatchEnd() { }
    protected virtual void OnInstantiate() { }
    protected virtual void OnPostInstantiate() { }
    protected virtual void OnUpdate() { }

    public virtual void Delete()
    {
        RemoveListeners();
        Destroy(gameObject);
    }

    protected GameState.State_Type GetGameState()
    {
        return GameStateFSM.GetGameState().type;
    }

}
