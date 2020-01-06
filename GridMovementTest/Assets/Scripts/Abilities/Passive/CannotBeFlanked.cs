using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CannotBeFlanked", menuName = "Unit Abilities/Passive/CannotBeFlanked")]
public class CannotBeFlanked : UnitAbility
{
    public override void ExecuteAbility(Unit abilityUser)
    {
        if (abilityUser.res.damageToBeTaken > 0)
        {
            abilityUser.res.damageToBeTaken = 1;
        }
    }
}
