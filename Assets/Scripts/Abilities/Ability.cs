using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{

    public Unit owner;
    public int range;
    public float amount;

    List<Tile> targettable_tiles = new List<Tile>();


    public void FindAffectableTiles()
    {
        Queue<Tile> process = new Queue<Tile>();

        Tile current_tile = owner.GetCurrentTile();
        current_tile.distance = 0;
        process.Enqueue(current_tile);
        while (process.Count > 0)
        {
            current_tile = process.Dequeue();
            if (current_tile.targettable || current_tile.distance > range) continue;
            if (IsAffectableTile(current_tile))
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

    public void RemoveAffectableTiles()
    {
        foreach (Tile tile in targettable_tiles)
        {
            tile.Reset();
        }
        targettable_tiles.Clear();
    }

    protected virtual bool IsAffectableTile(Tile tile)
    {
        return true;
    }

    public abstract void ApplyEffect(Unit target);
}
