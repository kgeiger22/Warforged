using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increases attack by X for Y rounds
public class ModifyAttack : Effect
{

    int amount;

    public ModifyAttack(Unit _owner, int _amount, int _duration) : base(_owner)
    {
        amount = _amount;
        duration = _duration;
        name = "Modify Attack";
    }

    public override void OnApply()
    {
        owner.ATK_modifier += amount;
    }

    public override void Delete()
    {
        owner.ATK_modifier -= amount;
        base.Delete();
    }
}

