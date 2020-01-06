using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LifeLeech", menuName = "Unit Abilities/Passive/LifeLeech")]
public class LifeLeech : UnitAbility
{
    [SerializeField] int value;

    public override void ExecuteAbility(Unit abilityUser)
    {
        Unit defendant = abilityUser.attackTile.unit;
        if (abilityUser.attackTile != null && defendant != null)
        {
            if (defendant.res.damageToBeTaken > 1) //if it has been backstabbed
            {
                abilityUser.res.RegainHealth(value);
            }
        }
    }
}
