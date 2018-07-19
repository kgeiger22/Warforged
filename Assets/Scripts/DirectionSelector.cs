using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSelector : MonoBehaviour {

    public WarforgedMonoBehaviour.Direction direction;

    private void OnMouseDown()
    {
        transform.parent.GetComponent<DirectionController>().SetDirection(direction);
    }
}
