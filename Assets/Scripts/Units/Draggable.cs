using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Draggable : WarforgedMonoBehaviour
{

    private static Material M_placeable = null;
    private static Material M_implaceable = null;

    private Unit S_unit;

    protected override void OnInstantiate()
    {
        S_unit = GetComponent<Unit>();
        if (!M_placeable) M_placeable = Resources.Load<Material>("Materials/Placeable");
        if (!M_implaceable) M_implaceable = Resources.Load<Material>("Materials/Implaceable");
    }

    protected override void OnUpdate()
    {
        Vector3 start = Camera.main.transform.position;
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 150));
        transform.position = Vector3.Lerp(start, target, start.y / Mathf.Abs(target.y - start.y));
        if (IsPlaceable()) S_unit.SetMaterials(M_placeable);
        else S_unit.SetMaterials(M_implaceable);
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
        }
    }

    public bool IsPlaceable()
    {
        if (Player.G_CURRENT_PLAYER.money - Unit.GetCost(S_unit.type) < 0) return false;
        if (!HoverManager.hovered || !HoverManager.hovered.IsWalkable() || !HoverManager.hovered.BelongsToCurrentPlayer())
        {
            if (!SelectionManager.selected || !SelectionManager.selected.IsWalkable() || !SelectionManager.selected.BelongsToCurrentPlayer()) 
            {
                return false;
            }
        }
        
        return true;
    }

    public void Place()
    {
        S_unit.Place();
    }
}
