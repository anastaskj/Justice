using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Regenerate", menuName = "Unit Abilities/Passive/Regenerate")]
public class Regenerate : UnitAbility
{
    [SerializeField]
    int healValue;

    public override void ExecuteAbility(Unit abilityUser)
    {
        abilityUser.RegainHealth(healValue);
    }
}
