using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LimitHealing", menuName = "Unit Abilities/Passive/LimitHealing")]
public class LimitHealing : UnitAbility
{
    public override void ExecuteAbility(Unit abilityUser)
    {
        Unit defendant = abilityUser.attackTile.unit;
        if (abilityUser.attackTile != null && defendant != null)
        {
            Debug.Log("Hmm");
            int value = defendant.res.GetMaxHealth() - 1;
            Debug.Log(value);
            defendant.res.SetMaxHealth(value);
            Debug.Log(defendant.res.GetMaxHealth());
        }
    }
}
