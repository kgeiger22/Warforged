using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRoundCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 2);
        transform.Find("Text").GetComponent<Text>().text = "Round " + BaseGame.round_number;
    }
	
}
