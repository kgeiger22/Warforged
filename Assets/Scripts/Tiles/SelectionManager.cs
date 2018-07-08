using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionManager {

    public static Tile selected { get; private set; }
    public static Tile hovered { get; private set; }

    public static void Select(Tile _select)
    {
        if (selected)
        {
            //double click
            if (selected == _select)
            {
                Camera.main.GetComponent<MainCamera>().MoveToTile(selected);
            }
            Unselect();
        }
        selected = _select;
        if (selected)
        {
            selected.OnSelect();
        }
        Unhover();
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
            selected.OnUnselect();
            selected = null;
        }
    }

    public static void Hover(Tile _hover)
    {
        if (hovered)
        {
            if (hovered == _hover) return;
            else Unhover();
        }
        if (_hover != GetSelectedTile()) hovered = _hover;
        if (hovered) hovered.Hover();
    }

    public static void Unhover()
    {
        if (hovered)
        {
            hovered.Unhover();
            hovered = null;
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
