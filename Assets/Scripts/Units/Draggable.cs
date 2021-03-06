﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Draggable : WarforgedMonoBehaviour
{

    private static Material M_placeable = null;
    private static Material M_implaceable = null;

    private Unit unit;

    protected override void OnInstantiate()
    {
        unit = GetComponent<Unit>();
        if (!M_placeable) M_placeable = Resources.Load<Material>("Materials/Placeable");
        if (!M_implaceable) M_implaceable = Resources.Load<Material>("Materials/Implaceable");
        SetPositionToMouse();
    }

    protected override void OnUpdate()
    {
        SetPositionToMouse();
        if (IsPlaceable()) unit.SetMaterials(M_placeable);
        else unit.SetMaterials(M_implaceable);
    }

    private void SetPositionToMouse()
    {
        Vector3 start = Camera.main.transform.position;
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 150));
        transform.position = Vector3.Lerp(start, target, start.y / Mathf.Abs(target.y - start.y));
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            string[] listofstrings = {
                "Tile",
            };
            int mask = LayerMask.GetMask(listofstrings);
            if (Physics.Raycast(ray, out hit, 200, mask))
            {
                unit.Place(hit.transform.gameObject.GetComponent<Tile>());
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            unit.Delete();
        }
    }

    public bool IsPlaceable()
    {
        if (!SelectionManager.hovered || !SelectionManager.hovered.IsWalkable() || !SelectionManager.hovered.BelongsToCurrentPlayer())
        {
            Tile tile = SelectionManager.GetSelectedTile();
            if (!tile || !tile.IsWalkable() || !tile.BelongsToCurrentPlayer()) 
            {
                return false;
            }
        }
        
        return true;
    }
}
