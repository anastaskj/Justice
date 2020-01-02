using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdjacentAttack", menuName = "Unit Abilities/Passive/AdjacentAttack")]
public class AdjacentUnitsAttack : UnitAbility
{
    public override void ExecuteAbility(Unit abilityUser)
    {
        IsometricTile origin = abilityUser.currentTile;
        IsometricTile attack = abilityUser.attackTile;

        foreach (IsometricTile i in origin.GetNeighbors())
        {
            foreach (IsometricTile ii in attack.GetNeighbors())
            {
                if (i == ii)
                {
                    if (i != null && i.unit != null && i.unit.champ.playerNumber == abilityUser.champ.playerNumber)
                    {
                        i.unit.res.hasAttacked = false;
                        i.unit.SetAttackTile(attack);
                        i.unit.StartAttack();
                        i.unit.res.hasAttacked = true;
                        attack.EnableAttackedIndicator(false);
                    }
                }
            }
        }

    }
}
