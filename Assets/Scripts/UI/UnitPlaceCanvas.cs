using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlaceCanvas : MonoBehaviour {

    List<UnitPlaceButton> buttons = new List<UnitPlaceButton>();

    float offset = 0f;
    float offset_size = -30f; //change for bigger/smaller space between buttons

	public void DisplayUnitsForCurrentPlayer()
    {
        Clear();
        foreach (Unit unit in Player.G_CURRENT_PLAYER.army)
        {
            AddUnitPlaceButton(unit);
        }
    }

    private void OnEnable()
    {
        if (BaseGame.G_GAMESTATEFSM != null && GameStateManager.GetGameState().type == GameState.State_Type.BUILD)  DisplayUnitsForCurrentPlayer();
    }

    public void AddUnitPlaceButton(UnitPlaceButton _button)
    {
        _button.GetComponent<RectTransform>().position += new Vector3(0, offset, 0);
        offset += offset_size;
        _button.index = buttons.Count;
        buttons.Add(_button);
    }

    public void AddUnitPlaceButton(Unit _unit)
    {
        UnitPlaceButton button = Instantiate(Resources.Load<UnitPlaceButton>("Prefabs/UnitPlaceButton"), transform);
        button.SetUnit(_unit);
        AddUnitPlaceButton(button);
    }

    public void RemovePlaceButton(int index)
    {
        Destroy(buttons[index].gameObject);
        buttons.RemoveAt(index);
        for(int i = index; i < buttons.Count; ++i)
        {
            buttons[i].index--;
            buttons[i].GetComponent<RectTransform>().position += new Vector3(0, -offset_size, 0);
        }
        offset -= offset_size;
    }

    public void Clear()
    {
        foreach(UnitPlaceButton button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        offset = 0f;
    }
}
