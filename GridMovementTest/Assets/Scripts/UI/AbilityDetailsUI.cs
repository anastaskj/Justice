using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityDetailsUI : MonoBehaviour
{
    public Text nameText;
    public Text descText;

    public void SetupAbilityUI(UnitAbility a)
    {
        if (a != null)
        {
            if (a is UnitActiveAbility)
            {
                nameText.text = a.abilityName + " (" + ((UnitActiveAbility)a).abilityRange + " range)";
            }
            else
            {
                nameText.text = a.abilityName;
            }
            descText.text = a.abilityDescription;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
