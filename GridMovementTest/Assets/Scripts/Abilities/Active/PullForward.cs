using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PullForward", menuName = "Unit Abilities/Active/PullForward")]
public class PullForward : UnitActiveAbility
{
    public override void ExecuteAbility(Unit abilityUser, IsometricTile tile)
    {
        if (tile != null && tile.unit && tile.unit.champ.playerNumber != abilityUser.champ.playerNumber)
        {
            Debug.Log("hmm");
            Unit target = tile.unit;
            //get the tile that is in front of the target from the perspective of the ability user
            IsometricTile pullDestination = target.currentTile.GetNeighbors()[(int)abilityUser.facingDirection.Opposite()];

            Debug.Log(pullDestination.coordinates.ToString());
            //only if the tile can be moved on
            if (pullDestination != null && pullDestination.CostToMove < 2)
            {
                Debug.Log("faas");
                target.currentTile.CostToMove = 1;
                target.currentTile.unit = null;

                target.StartMovement(target.TravelPath(new List<IsometricTile>() { target.currentTile, pullDestination }));
                pullDestination.SetUnit(target);
            }
        }

    }
}
