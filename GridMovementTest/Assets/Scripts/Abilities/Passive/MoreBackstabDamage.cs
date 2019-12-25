using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoreBackstab", menuName = "Unit Abilities/Passive/MoreBackstabDamage")]
public class MoreBackstabDamage : UnitAbility
{
    [SerializeField]
    int dmgIncrease;

    public override void ExecuteAbility(Unit abilityUser)
    {
        abilityUser.res.BackDamage += dmgIncrease;
    }
}
