using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUnitMove : MonoBehaviour {

    public void ClickEvent()
    {
        SelectionManager.GetSelectedUnit().SetState(new ReadyToMoveState(SelectionManager.GetSelectedUnit()));
        SelectionManager.Reselect();
    }
}
