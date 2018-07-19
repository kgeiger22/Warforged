using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour {

    Unit owner;

    public void SetOwner(Unit _owner)
    {
        owner = _owner;
    }

    public void SetDirection(WarforgedMonoBehaviour.Direction _direction)
    {
        owner.FaceDirection(_direction);
    }
}
