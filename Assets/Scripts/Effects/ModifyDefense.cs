using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Increases defense by X for Y rounds
public class ModifyDefense : Effect {

    int amount;

	public ModifyDefense(Unit _owner, int _amount, int _duration) : base(_owner)
    {
        amount = _amount;
        duration = _duration;
        name = "Modify Defense";
    }

    public override void OnApply()
    {
        owner.DEF_modifier += amount;
    }

    public override void Delete()
    {
        owner.DEF_modifier -= amount;
        base.Delete();
    }
}
