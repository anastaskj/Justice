using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushOnAttack", menuName = "Unit Abilities/Passive/PushOnAttack")]
public class PushOnAttack : UnitAbility
{
    public override void ExecuteAbility(Unit abilityUser)
    {
        Unit defendant = abilityUser.attackTile.unit;
        if (abilityUser.attackTile != null && defendant != null)
        {
            //only if the tile behind the one that the defender is currently on can be moved onto
            IsometricTile pushDestination = defendant.currentTile.GetOppositeNeighbor(abilityUser.facingDirection.Opposite());
            if (pushDestination != null && pushDestination.CostToMove < 2)
            {
                defendant.currentTile.CostToMove = 1;
                defendant.currentTile.unit = null;

                defendant.StartMovement(defendant.TravelPath(new List<IsometricTile>() { defendant.currentTile, pushDestination }));
                pushDestination.SetUnit(defendant);
            }
        }
    }
}
