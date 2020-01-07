using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Giantslayer", menuName = "Unit Abilities/Passive/Giantslayer")]
public class Giantslayer : UnitAbility
{
    [SerializeField] int value;

    public override void ExecuteAbility(Unit abilityUser)
    {
        Unit defendant = abilityUser.attackTile.unit;
        if (abilityUser.attackTile != null && defendant != null)
        {
            if (defendant.res.GetMaxHealth() > abilityUser.res.GetMaxHealth() + 1 && defendant.res.GetCurrentHealth() > 0)
            {
                if (!defendant.currentTile.GetNeighbors(defendant.facingDirection).Contains(abilityUser.currentTile))
                {
                    defendant.TakeDamage(value);
                }
            }
        }
    }
}
