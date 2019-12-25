using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanelUI : MonoBehaviour
{
    //think about where to get this from
    public IsometricUnitManager manager;

    //UI elements
    public Image unitSprite;
    public Text unitName, unitHealth, unitFlex, unitDmg, unitBackDmg;

    public RectTransform abilityListContent;
    public AbilityUIItem abilityImagePrefab;

    public AbilityDetailsUI abilityDetails;
    //

    private void Start()
    {
        UpdateUnitUI();
    }

    public void UpdateUnitUI()
    {
        Unit u = manager.selectedUnit;
        if (u != null)
        {
            unitSprite.sprite = u.sprite;
            unitName.text = u.tag;
            unitHealth.text = u.res.GetCurrentHealth() + "/" + u.res.GetMaxHealth();
            unitFlex.text = u.res.flexibility.ToString();
            unitDmg.text = u.res.SideDamage.ToString();
            unitBackDmg.text = u.res.BackDamage.ToString();

            FillList(u);
        }
        else
        {
            //default panel
        }
    }

    void FillList(Unit u)
    {
        for (int i = 0; i < abilityListContent.childCount; i++)
        {
            Destroy(abilityListContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < u.abilities.Count; i++)
        {
            UnitAbility a = u.abilities[i];
            AbilityUIItem item = Instantiate(abilityImagePrefab, abilityListContent, false);
            item.Sprite = a.sprite;
            item.ability = a;

            item.abilityDetailPrefab = abilityDetails;
        }
    }

    public void OnEnable()
    {
        UpdateUnitUI();
        manager.OnSelectedUnitChanged.AddListener(UpdateUnitUI);
    }

    private void OnDisable()
    {
        if (manager != null)
        {
            manager.OnSelectedUnitChanged.RemoveListener(UpdateUnitUI);
        }
    }
}
