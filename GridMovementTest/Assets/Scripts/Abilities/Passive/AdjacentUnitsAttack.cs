using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "AdjacentAttack", menuName = "Unit Abilities/Passive/AdjacentAttack")]
public class AdjacentUnitsAttack : UnitAbility
{
    [SerializeField] int value;

    public override void ExecuteAbility(Unit abilityUser)
    {
        //IsometricTile origin = abilityUser.currentTile;
        //IsometricTile attack = abilityUser.attackTile;

        //foreach (IsometricTile i in origin.GetNeighbors())
        //{
        //    foreach (IsometricTile ii in attack.GetNeighbors())
        //    {
        //        if (i == ii)
        //        {
        //            if (i != null && i.unit != null && i.unit.champ.playerNumber == abilityUser.champ.playerNumber)
        //            {
        //                i.unit.res.hasAttacked = false;
        //                i.unit.SetAttackTile(attack);
        //                i.unit.StartAttack();
        //                i.unit.res.hasAttacked = true;
        //                attack.EnableAttackedIndicator(false);
        //            }
        //        }
        //    }
        //}

        IsometricTile origin = abilityUser.currentTile;
        foreach (IsometricTile t in origin.GetNeighbors(abilityUser.facingDirection))
        {
            //t.grid.HighlightTiles(origin.GetNeighbors(abilityUser.facingDirection).ToList(), true);
            if (t != abilityUser.attackTile)
            {
                if (t.unit != null && t.unit.champ.playerNumber != abilityUser.champ.playerNumber)
                {
                    if (!t.unit.currentTile.GetNeighbors(t.unit.facingDirection).Contains(abilityUser.currentTile))
                    {
                        t.unit.TakeDamage(value);
                    }
                }
            }
        }
    }
}
