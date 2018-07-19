using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDeleteUnit : MonoBehaviour {

    Button S_button;

    private void Awake()
    {
        S_button = GetComponent<Button>();
    }

    private void Update()
    {
        if (SelectionManager.GetSelectedUnit() && SelectionManager.GetSelectedUnit().owner == PlayerManager.CurrentPlayer) S_button.interactable = true;
        else S_button.interactable = false;
    }

    public void DeleteUnit()
    {
        SelectionManager.GetSelectedUnit().Delete();
    }
}
