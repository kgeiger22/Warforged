using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionManager {

    private static Tile selected = null;

    public static void Select(Tile _select)
    {
        if (selected)
        {
            //double click
            if (selected == _select)
            {
                Camera.main.GetComponent<MainCamera>().MoveToTile(selected);
                return;
            }
            Unselect();
        }
        selected = _select;
        if (selected)
        {
            selected.Select();
        }
        HoverManager.Unhover();
    }

    public static void Reselect()
    {
        if (selected)
        {
            Tile t = selected;
            Unselect();
            Select(t);
        }
    }

    public static void Unselect()
    {
        if (selected)
        {
            selected.Unselect();
            selected = null;
        }
    }

    public static Tile GetSelectedTile()
    {
        return selected;
    }

    public static Unit GetSelectedUnit()
    {
        if (selected) return selected.unit;
        else return null;
    }
}
