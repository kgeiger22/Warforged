using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class DamageText : MonoBehaviour {

    public float moveSpeed = 1f;
    public float duration = 1f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.up * Time.deltaTime * moveSpeed;
	}

    public void SetText(string _text)
    {
        GetComponent<TextMesh>().text = _text;
    }
}
