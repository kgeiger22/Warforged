using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplifyDamageReceived : Effect {

    float multiplier = 1.0f;

	public AmplifyDamageReceived(Unit _owner, float _multiplier) : base(_owner)
    {
        multiplier = _multiplier;
        duration = 1;
        type = Type.RECEIVE_DAMAGE;
        name = "Amplify Damage";
    }

    public override void Activate(ref float val)
    {
        val *= multiplier;
    }
}
