using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextState : MonoBehaviour {

    Text text;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update () {
        if (text != null) text.text = GameStateFSM.GetGameState().type.ToString();
        if (PlayerManager.CurrentPlayer == Player.Type.PLAYER1)
        {
            image.color = new Color(1, 0.5f, 1);
        }
        else if (PlayerManager.CurrentPlayer == Player.Type.PLAYER2)
        {
            image.color = new Color(1, 0.75f, 0.4f);
        }
        else
        {
            image.color = Color.white;
        }
	}

    public void NextState()
    {
        Global.GAMESTATEFSM.NextState();
    }
}
