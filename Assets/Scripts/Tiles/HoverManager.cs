﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HoverManager {

    public static Tile hovered = null;

    public static void Hover(Tile _hover)
    {
        if (hovered)
        {
            if (ReferenceEquals(hovered, _hover)) return;
            else Unhover();
        }
        if (!ReferenceEquals(_hover, SelectionManager.selected)) hovered = _hover;
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
}