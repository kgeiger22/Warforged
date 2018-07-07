using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCanvas : MonoBehaviour
{

    List<ButtonAbility> buttons = new List<ButtonAbility>();

    float offset = 0f;
    float offset_size = -30f; //change for bigger/smaller space between buttons

    public void DisplayAbilitiesForCurrentUnit()
    {
        Clear();
        List<Ability> abilities = SelectionManager.GetSelectedUnit().abilities;
        for (int i = 0; i < abilities.Count; ++i)
        {
            AddAbilityButton(i, abilities[i].name);
        }
    }

    private void OnEnable()
    {
        if (BaseGame.G_GAMESTATEFSM != null && GameStateManager.GetGameState().type == GameState.State_Type.TURN) DisplayAbilitiesForCurrentUnit();
    }

    public void AddAbilityButton(ButtonAbility _button)
    {
        _button.GetComponent<RectTransform>().position += new Vector3(0, offset, 0);
        offset += offset_size;
        buttons.Add(_button);
    }

    public void AddAbilityButton(int _index, string _name)
    {
        ButtonAbility button = Instantiate(Resources.Load<ButtonAbility>("Prefabs/AbilityButton"), transform.Find("InfoHolder"));
        button.Initialize(SelectionManager.GetSelectedUnit(), _index, _name);
        AddAbilityButton(button);
    }


    public void Clear()
    {
        foreach (ButtonAbility button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
        offset = 0f;
    }
}