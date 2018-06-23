using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonGenerateUnit : MonoBehaviour {

    [SerializeField]
    Unit.Type type;

    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => UnitFactory.GenerateUnit(type, null));

        transform.Find("Text").GetComponent<Text>().text = type.ToString() + " (" + Unit.GetCost(type).ToString() + ")";
    }

    private void Update()
    {
        if (GameState.G_GAMESTATE.type != GameState.State_Type.BUILD)
        {
            button.interactable = false;
        }
        else if (Player.G_CURRENT_PLAYER.money < Unit.GetCost(type))
        {
            button.interactable = false;
        }
        else button.interactable = true;
    }
}
