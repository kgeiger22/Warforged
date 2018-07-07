using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonUnitAttack : MonoBehaviour
{

    public void ClickEvent()
    {
        SelectionManager.GetSelectedUnit().SetState(new ReadyToAttackState(SelectionManager.GetSelectedUnit()));
        SelectionManager.GetSelectedUnit().SelectTiles();
    }
}
