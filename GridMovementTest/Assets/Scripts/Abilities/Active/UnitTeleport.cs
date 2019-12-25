using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Teleport", menuName = "Unit Abilities/Active/Teleport")]
public class UnitTeleport : UnitActiveAbility
{
    public override void ExecuteAbility(Unit abilityUser, IsometricTile target)
    {
        if (target != null && target.CostToMove < 2) //empty tiles only
        {
            abilityUser.grid.ShowTacticPattern(abilityUser.possibleTiles, false);

            abilityUser.currentTile.CostToMove = 1;
            abilityUser.currentTile.unit = null;

            target.SetUnit(abilityUser);

            abilityUser.transform.localPosition = abilityUser.currentTile.transform.position;
            abilityUser.InitializeTactic();
        }
    }
}
