using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDetailsUI : MonoBehaviour
{
    public Text healthText;
    public Text flexText;
    public Text unitName;
    public Image tacticImage;

    public RectTransform abilityListContent;
    public AbilityUIItem abilityImagePrefab;

    public AbilityDetailsUI abilityDetails;
    public Vector3 spawnOffset;

    [SerializeField]
    Color normalHealthColor;
    [SerializeField]
    Color shieldedHealthColor;

    public void SetupDetails(Unit u)
    {
        healthText.text = u.res.GetCurrentHealth().ToString();
        if (u.res.GetCurrentHealth() > u.res.GetMaxHealth())
        {
            healthText.color = shieldedHealthColor;
        }
        else
        {
            healthText.color = normalHealthColor;
        }
        flexText.text = u.res.flexibility.ToString();
        unitName.text = u.tag;
        tacticImage.sprite = u.tactic.picture;
        FillList(u);
        this.transform.position = Input.mousePosition + spawnOffset;
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

            //Debug.Log("ey rectLocal: " + item.rect.transform.localPosition);
            //Debug.Log("ey rectPos: " + item.rect.transform.position);
        }
    }
}
