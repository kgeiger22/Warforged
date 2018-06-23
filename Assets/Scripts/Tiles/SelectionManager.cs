using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectionManager {

    public static Tile selected = null;

    public static void Select(Tile _select)
    {
        if (selected)
        {
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
            Select(selected);
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
}
