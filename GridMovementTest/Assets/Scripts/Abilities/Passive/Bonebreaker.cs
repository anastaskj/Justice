using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Bonebreaker", menuName = "Unit Abilities/Passive/Bonebreaker")]
public class Bonebreaker : UnitAbility
{
    [SerializeField] int value;

    public override void ExecuteAbility(Unit abilityUser)
    {
        Unit defendant = abilityUser.attackTile.unit;
        if (abilityUser.attackTile != null && defendant != null)
        {
            if (defendant.res.GetCurrentHealth()+1 < abilityUser.res.GetCurrentHealth() && defendant.res.GetCurrentHealth() > 0)
            {
                if (!defendant.currentTile.GetNeighbors(defendant.facingDirection).Contains(abilityUser.currentTile))
                {
                    defendant.TakeDamage(value);
                }
            }
        }
    }
}
