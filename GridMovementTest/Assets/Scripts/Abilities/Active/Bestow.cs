using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bestow", menuName = "Unit Abilities/Active/Bestow")]
public class Bestow : UnitActiveAbility
{
    [SerializeField]
    int shieldValue;

    public override void ExecuteAbility(Unit abilityUser, IsometricTile target)
    {
        if (target != null && target.unit != null && target.unit.champ.playerNumber == abilityUser.champ.playerNumber) //friendly only
        {
            target.unit.res.GainShield(shieldValue);
            target.unit.res.OnValueChanged.Invoke();
        }
    }
}
