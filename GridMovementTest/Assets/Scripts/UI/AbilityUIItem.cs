using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AbilityDetailsUI abilityDetailPrefab;
    public UnitAbility ability;
    public RectTransform rect;

    Sprite sprite;
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
        set
        {
            sprite = value;
            GetComponent<Image>().sprite = sprite;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ability)
        {
            abilityDetailPrefab.SetupAbilityUI(ability);
            abilityDetailPrefab.Show();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ability)
        {
            abilityDetailPrefab.Close();
        }
    }
}
