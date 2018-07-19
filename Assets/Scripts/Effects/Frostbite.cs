using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slow movement by 2 for 2 rounds
public class Frostbite : Effect {

    public Frostbite(Unit _owner) : base(_owner)
    {
        duration = 2;
        name = "Frostbite";
    }

    public override void OnApply()
    {
        owner.SPD_modifier -= 2;
        owner.ModifyMovesRemaining(-2);
    }

    public override void Delete()
    {
        owner.SPD_modifier += 2;
        base.Delete();
    }
}
