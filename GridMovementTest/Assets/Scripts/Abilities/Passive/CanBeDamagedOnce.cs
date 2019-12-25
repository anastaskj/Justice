using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamagedOnce", menuName = "Unit Abilities/Passive/CanBeDamagedOnce")]
public class CanBeDamagedOnce : UnitAbility
{
    public override void ExecuteAbility(Unit abilityUser)
    {
        bool canBeDamaged = abilityUser.res.hasTakenDamage;
        if (canBeDamaged)
        {
            abilityUser.res.damageToBeTaken = 0;
        }
    }
}
