using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoCanvas : MonoBehaviour {

    Text unit_type;
    Text unit_hp;
    Text unit_moves;


    private void Awake()
    {
        unit_type = transform.Find("InfoHolder").Find("UnitType").GetComponent<Text>();
        unit_hp = transform.Find("InfoHolder").Find("UnitHP").GetComponent<Text>();
        unit_moves = transform.Find("InfoHolder").Find("UnitMoves").GetComponent<Text>();

    }

    private void Update()
    {
        UpdateInfo(SelectionManager.GetSelectedUnit());
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
            unit_hp.text = "HP: " + unit.health + "/" + unit.HP;
            unit_moves.text = "Moves: " + unit.moves_remaining + "/" + unit.SPD;

        }

    }
}
