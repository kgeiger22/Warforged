using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{

    public enum Type
    {
        DAMAGE,
        STASUS,
        SUPPORT,
        PASSIVE,
    }
    public enum SubType
    {
        MELEE,
        RANGED,
        MAGIC,
        NERF,
        BUFF,
        TERRAIN,
    }

    public string name;
    protected Unit owner;
    public int range;
    public bool instant_execute = false;

    public virtual void HighlightTargettableTiles()
    {
        Board.ResetAllTiles();

        Queue<Tile> process = new Queue<Tile>();

        Tile current_tile = owner.GetCurrentTile();
        current_tile.distance = 0;
        process.Enqueue(current_tile);
        while (process.Count > 0)
        {
            current_tile = process.Dequeue();
            if (current_tile.targettable || current_tile.distance > range) continue;
            else
            {
                if (IsTargettableTile(current_tile))
                {
                    current_tile.targettable = true;
                    if (IsValidTile(current_tile))
                    {
                        current_tile.valid = true;
                    }
                }

                foreach (Tile next_tile in current_tile.adjacency_list)
                {
                    next_tile.distance = current_tile.distance + 1;
                    next_tile.parent = current_tile;
                    process.Enqueue(next_tile);

                }
            }
        }
    }

    //override to change which tiles could potentially be targetted regardless of unit placement
    protected virtual bool IsTargettableTile(Tile tile)
    {
        return true;
    }

    //override to change valid targets
    protected virtual bool IsValidTile(Tile tile)
    {
        return (tile.unit && !tile.unit.BelongsToCurrentPlayer());
    }

    public abstract void Execute(Unit target);

    protected virtual void ApplyEffect(Unit target, Effect effect)
    {
        target.ApplyEffect(effect);
    }

    //damage paramater is AFTER attack calculation but BEFORE defense mitigation
    //override for attacks that calculate damage differently
    protected virtual void DealDamage(Unit target, float damage)
    {
        float multiplier = 1.0f;
        if (WarforgedMonoBehaviour.AreSameDirection(owner.direction, target.direction))
        {
            Debug.Log("backstab");
            multiplier = 1.2f;
        }
        target.ReceiveDamage(damage * multiplier * owner.ATK * ((100f - target.DEF) * 0.01f));
    }
}
