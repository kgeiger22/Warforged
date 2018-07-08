using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    public float move_speed = 50;
    public float moveto_speed = 50;

    private bool moveto = false;
    private Vector3 target;
    private Vector3 velocity;
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (Mathf.Abs(h) > 0.01f || Mathf.Abs(v) > 0.01f) moveto = false;
        transform.position += new Vector3(h * Time.deltaTime * move_speed, 0, v * Time.deltaTime * move_speed);

        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(mw) > 0.01f) moveto = false;
        transform.position += transform.forward * mw * 100;

        if (moveto)
        {
            Vector3 newposition = Vector3.SmoothDamp(transform.position, target, ref velocity, 0.25f, moveto_speed, Time.deltaTime);
            newposition.y = transform.position.y;
            transform.position = newposition;
            if (velocity.magnitude < 0.01f) moveto = false;
        }
    }

    public void MoveToTile(Tile _tile)
    {
        float difz = _tile.transform.position.z - transform.position.z;
        float dify = _tile.transform.position.y - transform.position.y;
        target = _tile.transform.position - transform.forward * Mathf.Sqrt(difz * difz + dify * dify);
        moveto = true;
    }
}
