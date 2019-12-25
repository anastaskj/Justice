using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionUI : MonoBehaviour
{
    //think about where to get unit from
    public IsometricUnitManager manager;

    //UI elements
    public Button[] buttons;

    public Button abilityButton;
    public Text abilityText;

    public Button movementButton;

    public Button dirEast, dirWest, dirNorth, dirSouth;

    public Text tacticName;
    public Image tacticSprite;

    public AbilityDetailsUI abilityDetails;
    public AbilityUIItem ability;

    public Sprite noAbilitySprite;

    public GameObject movementHighlight;
    public GameObject abilityHighlight;
    //

    private void Start()
    {
        UpdateUI();
    }

    public void HighlightAction()
    {
        if (manager.MoveActive)
        {
            movementHighlight.SetActive(true);
            abilityHighlight.SetActive(false);
        }
        else if (manager.UseAbilityActive)
        {
            movementHighlight.SetActive(false);
            abilityHighlight.SetActive(true);
        }
        else //changed positions
        {
            movementHighlight.SetActive(false);
            abilityHighlight.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        Unit selected = manager.selectedUnit;
        if (selected != null)
        {
            tacticName.text = selected.tactic.name;
            tacticSprite.sprite = selected.tactic.picture;

            if (selected.res.activeAbility != null)
            {
                abilityButton.image.sprite = selected.res.activeAbility.sprite;
                abilityText.text = selected.res.activeAbility.abilityName;

                ability.ability = selected.res.activeAbility;
                ability.abilityDetailPrefab = abilityDetails;
            }
            else
            {
                abilityButton.image.sprite = noAbilitySprite;
                abilityText.text = "No ability";
            }

            if (selected.res.ActionPoints == 0)
            {
                foreach (Button b in buttons)
                {
                    b.interactable = false;
                }
            }
            else
            {
                if (selected.res.usedAbility)
                {
                    abilityButton.interactable = false;
                }
                else
                {
                    abilityButton.interactable = true;
                }

                if (selected.res.willMove)
                {
                    movementButton.interactable = false;
                }
                else
                {
                    movementButton.interactable = true;
                }

                dirEast.interactable = true;
                dirWest.interactable = true;
                dirSouth.interactable = true;
                dirNorth.interactable = true;
            }
        }
    }

    public void OnEnable()
    {
        UpdateUI();
        manager.OnSelectedUnitChanged.AddListener(UpdateUI);
        manager.OnActionChanged.AddListener(HighlightAction);
    }

    private void OnDisable()
    {
        if (manager != null)
        {
            manager.OnSelectedUnitChanged.RemoveListener(UpdateUI);
            manager.OnActionChanged.RemoveListener(HighlightAction);
        }
    }
}
