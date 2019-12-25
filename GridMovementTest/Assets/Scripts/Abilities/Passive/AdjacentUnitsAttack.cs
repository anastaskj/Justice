using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdjacentAttack", menuName = "Unit Abilities/Passive/AdjacentAttack")]
public class AdjacentUnitsAttack : UnitAbility
{
    public override void ExecuteAbility(Unit abilityUser)
    {
        IsometricTile origin = abilityUser.attackTile;

        foreach (IsometricTile i in origin.GetNeighbors())
        {
            if (i && i.unit)
            {
                if (i.unit.champ.playerNumber == abilityUser.champ.playerNumber) //only friendly units
                {
                    //i.unit.res.hasAttacked = false;
                    //i.unit.SetAttackTile(origin);
                    //i.unit.AttackTile();
                    ////i.unit.StartAnimation(i.unit.PerformAttack());

                    //origin.EnableAttackedIndicator(false);
                }
            }
        }

    }
}
