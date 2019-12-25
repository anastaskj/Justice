using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StunEnemy", menuName = "Unit Abilities/Active/StunEnemy")]
public class StunEnemy : UnitActiveAbility
{
    public override void ExecuteAbility(Unit abilityUser, IsometricTile target)
    {
        if (target != null && target.unit.champ.playerNumber != abilityUser.champ.playerNumber) //enemy units only
        {
            target.unit.res.ActionPoints = 0;
        }
    }
}
