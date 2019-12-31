using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamagedOnce", menuName = "Unit Abilities/Passive/CanBeDamagedOnce")]
public class CanBeDamagedOnce : UnitAbility
{
    [SerializeField]
    int shieldAmount = 1;

    public override void ExecuteAbility(Unit abilityUser)
    {
        abilityUser.res.GainShield(shieldAmount);
    }
}
