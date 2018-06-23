using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public float move_speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.position += new Vector3(h * Time.deltaTime * move_speed, 0, v * Time.deltaTime * move_speed);

        float mw = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.forward * mw * 100;
    }
}
