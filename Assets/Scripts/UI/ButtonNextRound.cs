using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextRound : MonoBehaviour {

    Text text;
    Button button;
    public int i = 0;

    private void Awake()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (text != null) text.text = "Round" + i;
        if (GameStateManager.GetGameState().type == GameState.State_Type.TURN)
        {
            button.interactable = true;
        }
        else button.interactable = false;
    }

    public void NextRound()
    {
        BaseGame.G_GAMESTATEFSM.SetState(new RoundState());
    }
}
