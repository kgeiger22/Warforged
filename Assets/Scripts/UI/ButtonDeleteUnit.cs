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
        if (SelectionManager.selected && SelectionManager.selected.unit && ReferenceEquals(SelectionManager.selected.unit.owner, Player.G_CURRENT_PLAYER)) S_button.interactable = true;
        else S_button.interactable = false;
    }

    public void DeleteUnit()
    {
        SelectionManager.selected.DeleteUnit();
    }
}
