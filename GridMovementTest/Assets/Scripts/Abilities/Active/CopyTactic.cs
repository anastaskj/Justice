using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CopyTactic", menuName = "Unit Abilities/Active/CopyTactic")]
public class CopyTactic : UnitActiveAbility
{
    public override void ExecuteAbility(Unit abilityUser, IsometricTile target)
    {
        if (target != null) //any unit
        {
            Tactics t = target.unit.tactic;
            if (abilityUser.res.flexibility >= t.flexibilityRequirement)
            {
                //reset current tactic
                abilityUser.grid.ShowTacticPattern(abilityUser.InitializeTactic(), false);
                //update to new
                abilityUser.tactic = t;
                abilityUser.grid.ShowTacticPattern(abilityUser.InitializeTactic(), true);
            }
        }
    }
}
