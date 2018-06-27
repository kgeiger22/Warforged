using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour {

    public int duration;

	public virtual void OnApply()
    {

    }

    public virtual void OnEndOfTurn()
    {

    }
}
