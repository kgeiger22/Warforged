using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoCanvas : MonoBehaviour {

    Text unit_type;

    private void Awake()
    {
        unit_type = transform.Find("InfoHolder").Find("UnitType").GetComponent<Text>();
    }

    private void OnEnable()
    {
        if (SelectionManager.selected)
        {
            UpdateInfo(SelectionManager.selected.unit);
        }
    }

    public void UpdateInfo(Unit unit)
    {
        if (unit == null)
        {
            unit_type.text = "NULL";
        }
        else
        {
            unit_type.text = "Type: " + unit.type.ToString();
        }

    }
}
