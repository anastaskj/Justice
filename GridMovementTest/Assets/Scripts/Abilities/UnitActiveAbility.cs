using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Ability", menuName = "Unit Abilities/Active")]
public class UnitActiveAbility : UnitAbility
{
    public int abilityRange;

    public virtual void ExecuteAbility(Unit abilityUser, IsometricTile target){ }
}
