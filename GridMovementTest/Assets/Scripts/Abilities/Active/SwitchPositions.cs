using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwitchPositions", menuName = "Unit Abilities/Active/SwitchPositions")]
public class SwitchPositions : UnitActiveAbility
{
    public override void ExecuteAbility(Unit abilityUser, IsometricTile tile)
    {
        if (tile != null && tile.unit.champ.playerNumber != abilityUser.champ.playerNumber)
        {
            Unit target = tile.unit;
            abilityUser.grid.ShowTacticPattern(abilityUser.possibleTiles, false);

            IsometricTile temp = target.currentTile;
            abilityUser.currentTile.SetUnit(target);
            temp.SetUnit(abilityUser);

            abilityUser.transform.localPosition = abilityUser.currentTile.transform.position;
            target.transform.localPosition = target.currentTile.transform.position;

            abilityUser.InitializeTactic();
        }
    }
}
