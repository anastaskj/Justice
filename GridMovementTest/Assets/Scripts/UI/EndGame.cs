using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text endText;
    public Button nextBattle;
    public Button quit;

    public AbilityUIItem abilityPrefab;
    public GameObject panelUnlock;
    public Text unlockName;
    public Image unlockImage;
    public Transform container;
    public AbilityDetailsUI abilityDetails;

    public Unit[] kingdomUnits;
    public Unit[] virulentUnits;

    public void SetText(int playerWon)
    {
        if (playerWon == 1)
        {
            endText.text = "Defeat";
            nextBattle.gameObject.SetActive(false);
        }
        else
        {
            ProgressState state = IsometricMetrics.progress + 1;
            string progress = state.ToString();
            endText.text = "Victory! \n You have reached the " + state;
            nextBattle.gameObject.SetActive(true);
            if (IsometricMetrics.progress == ProgressState.Final)
            {
                endText.text = "You have won the tournament! \n Your people will prosper!";
                nextBattle.gameObject.SetActive(false);
                panelUnlock.SetActive(false);
            }
            else
            {
                ShowUnlock(IsometricMetrics.progress, IsometricMetrics.chosenFaction);
            }
        }
    }


    void ShowUnlock(ProgressState progress, Factions faction)
    {
        switch (faction)
        {
            case Factions.Kingdom:
                LoadUnit(kingdomUnits[(int)progress]);
                break;
            case Factions.Virulent:
                LoadUnit(virulentUnits[(int)progress]);
                break;
            default:
                break;
        }
    }

    void LoadUnit(Unit u)
    {
        unlockName.text = "The " + u.tag + " has joined your army!";
        unlockImage.sprite = u.sprite;
        for (int i = 0; i < u.abilities.Count; i++)
        {
            UnitAbility a = u.abilities[i];
            AbilityUIItem item = Instantiate(abilityPrefab, container, false);
            item.Sprite = a.sprite;
            item.ability = a;

            item.abilityDetailPrefab = abilityDetails;
        }
        if (u.res.activeAbility)
        {
            UnitAbility a = u.res.activeAbility;
            AbilityUIItem item = Instantiate(abilityPrefab, container, false);
            item.Sprite = a.sprite;
            item.ability = a;

            item.abilityDetailPrefab = abilityDetails;
        }
    }
   
}
