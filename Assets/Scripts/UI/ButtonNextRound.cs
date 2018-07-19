using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextRound : MonoBehaviour {

    Text text;
    Button button;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (text != null) text.text = "Round" + BaseGame.round_number;
        if (GameStateFSM.GetGameState().type == GameState.State_Type.TURN)
        {
            button.interactable = true;
        }
        else button.interactable = false;
    }

    public void NextRound()
    {
        Global.GAMESTATEFSM.SetState(new RoundState());
    }
}
