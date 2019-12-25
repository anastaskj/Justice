using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Ability", menuName = "Unit Abilities/Passive")]
public abstract class UnitAbility : ScriptableObject
{
    public AbilityTrigger trigger;
    public string abilityName;
    public string abilityDescription;
    public Sprite sprite;
    public virtual void ExecuteAbility(Unit abilityUser) { }
}

public enum AbilityTrigger
{
    ON_ATTACK, ON_DEFENSE, ON_START_GAME, ON_END_TURN, ACTIVE
}
