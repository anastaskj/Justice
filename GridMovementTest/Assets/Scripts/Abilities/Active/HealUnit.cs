using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealUnit", menuName = "Unit Abilities/Active/Heal")]
public class HealUnit : UnitActiveAbility
{
    [SerializeField]
    int healValue;

    public override void ExecuteAbility(Unit abilityUser, IsometricTile target)
    {
        if (target != null && target.unit != null && target.unit.champ.playerNumber == abilityUser.champ.playerNumber) //friendly only
        {
            target.unit.RegainHealth(healValue);
        }
    }
}
