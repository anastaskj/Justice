using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitResources : MonoBehaviour
{
    [SerializeField]
    int maxHealth;
    [SerializeField]
    int currentHealth;
    [SerializeField]
    int sideDamage;
    [SerializeField]
    int backDamage;
    [SerializeField]
    int actionPoints;
    [SerializeField]
    public int flexibility;
    [SerializeField]
    public float speed = 3f;
    [SerializeField]
    public float attackTimer = 1f; //move this to anim class
    [SerializeField]
    public float deathTimer = 1f; //move this to anim class

    public bool hasMoved = false;
    public bool hasAttacked = false;
    public bool hasChangeDir = false;

    public bool willMove = false;
    public bool willAttack = false;

    public bool hasTakenDamage;
    public bool usedAbility;

    public Color playerColor;

    public UnitActiveAbility activeAbility;

    public int damageToBeTaken = 0;
   

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public UnityEvent OnValueChanged = new UnityEvent();


    public int ActionPoints
    {
        get
        {
            return actionPoints;
        }
        set
        {
            actionPoints = value;
            if (actionPoints > 2)
            {
                actionPoints = 2;
            }
        }
    }

    public int BackDamage
    {
        get
        {
            return backDamage;
        }
        set
        {
            backDamage = value;
        }
    }

    public int SideDamage
    {
        get
        {
            return sideDamage;
        }
        set
        {
            sideDamage = value;
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hasTakenDamage = true;
        OnValueChanged.Invoke();
    }

    public void RegainHealth(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnValueChanged.Invoke();
    }

    public void MakeAction()
    {
        actionPoints -= 1;
    }
}
