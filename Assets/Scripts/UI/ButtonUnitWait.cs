using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUnitWait : MonoBehaviour
{

    public void ClickEvent()
    {
        Unit unit = SelectionManager.GetSelectedUnit();
        unit.EndTurn();
    }
}
