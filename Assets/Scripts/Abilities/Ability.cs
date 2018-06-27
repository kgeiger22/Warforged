using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{

    public Unit owner;
    public int range;

    List<Tile> targettable_tiles = new List<Tile>();

    public virtual void FindTargettableTiles()
    {
        //clear previous tiles
        owner.RemoveSelectableTiles();

        Queue<Tile> process = new Queue<Tile>();

        Tile current_tile = owner.GetCurrentTile();
        current_tile.distance = 0;
        process.Enqueue(current_tile);
        while (process.Count > 0)
        {
            current_tile = process.Dequeue();
            if (current_tile.targettable || current_tile.distance > range) continue;
            if (IsTargettableTile(current_tile))
            {
                targettable_tiles.Add(current_tile);
                current_tile.targettable = true;
            }

            foreach (Tile next_tile in current_tile.adjacency_list)
            {
                next_tile.distance = current_tile.distance + 1;
                process.Enqueue(next_tile);
            }
        }
    }

    public void RemoveTargettableTiles()
    {
        foreach (Tile tile in targettable_tiles)
        {
            tile.Reset();
        }
        targettable_tiles.Clear();
    }

    //override to change which tiles could potentially be targetted regardless of unit placement
    protected virtual bool IsTargettableTile(Tile tile)
    {
        return true;
    }

    //override to change valid targets
    protected virtual bool IsValidTile(Tile tile)
    {
        return (!tile.unit.BelongsToCurrentPlayer());
    }

    public virtual void Resolve(Unit target)
    {

    }

    protected virtual void ApplyEffect(Unit target, Effect effect)
    {
        target.ApplyEffect(effect);
    }

    //damage paramater is AFTER attack calculation but BEFORE defense mitigation
    //override for attacks that calculate damage differently
    protected virtual void DealDamage(Unit target, float damage)
    {

    }
}
