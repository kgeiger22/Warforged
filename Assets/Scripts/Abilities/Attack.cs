using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : Ability {

    public enum AttackType
    {
        MELEE,
        RANGED,
    }

	public enum DamageType
    {
        PHYSICAL,
        MAGICAL,
        STATUS,
    }

    public AttackType attack_type;
    public DamageType damage_type;

    public Attack(Unit _owner) : base(_owner)
    {
        type = Type.ATTACK;
    }

    //override to change valid targets
    protected override bool IsValidTile(Tile tile)
    {
        return (tile.unit && !tile.unit.BelongsToCurrentPlayer());
    }

    //damage paramater is AFTER attack calculation but BEFORE defense mitigation
    //override for attacks that calculate damage differently
    protected virtual void DealDamage(Unit target, float damage)
    {
        float accuracy_roll = Random.Range(0f, 1f);
        float owner_accuray = owner.GetModifiedACC();
        if (accuracy_roll > owner_accuray)
        {
            //miss
            target.ReceiveDamage(-1f);
            return;
        }

        float damage_multiplier = 1.0f;
        if (WarforgedMonoBehaviour.AreSameDirection(owner.direction, target.direction))
        {
            Debug.Log("Backstab");
            damage_multiplier = 1.2f;
        }
        int attacker_rally_bonus = owner.CalculateRallyBonus();
        if (attacker_rally_bonus > 0) Debug.Log("Attacker Rally + " + attacker_rally_bonus);
        int defender_rally_bonus = target.CalculateRallyBonus();
        if (defender_rally_bonus > 0) Debug.Log("Defender Rally + " + defender_rally_bonus);

        target.ReceiveDamage(damage * damage_multiplier * (owner.GetModifiedATK() + attacker_rally_bonus) * ((100f - (target.GetModifiedDEF() + defender_rally_bonus)) * 0.01f));
    }
}
