using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIMoney : MonoBehaviour {

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        if (PlayerManager.GetCurrentPlayer()) text.text = PlayerManager.GetCurrentPlayer().money.ToString();
	}
}
