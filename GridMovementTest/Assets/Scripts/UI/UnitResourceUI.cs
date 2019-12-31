using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitResourceUI : MonoBehaviour
{
    public Image playerColor;

    public Slider healthBar;
    public Text healthText;

    UnitResources res;
    
    public Text damageLabel;
    public Canvas canv;
    
    [SerializeField]
    Color normalHealthColor;
    [SerializeField]
    Color shieldedHealthColor;

    public void ShowDamageLabel(int damageNumber)
    {
        Text label = Instantiate<Text>(damageLabel);
        label.rectTransform.SetParent(canv.transform, false);
        label.rectTransform.localPosition = this.transform.localPosition;
        label.text = "-" + damageNumber.ToString();
    }

    public void ShowHealthLabel(int healNumber)
    {
        Text label = Instantiate<Text>(damageLabel);
        label.color = Color.green;
        label.rectTransform.SetParent(canv.transform, false);
        label.rectTransform.localPosition = this.transform.localPosition;
        label.text = "+" + healNumber.ToString();
        
    }

    public void ShowDirectionArrow(IsometricDirection dir) //replace with event
    {
        GameObject exception = null;
        GameObject[] children = new GameObject[4];

        for (int i = 0, j = 1; i < 8; i += 2, j++) //n, w, s, e - 0, 2, 4, 6
        {
            if (i == (int)dir)
            {
                exception = canv.transform.GetChild(j).gameObject; //child based on index
            }
            children[j-1] = canv.transform.GetChild(j).gameObject;
        }
        ResetGameObjects(exception, children);
    }

    void ResetGameObjects(GameObject exception, GameObject[] collection)
    {
        foreach (GameObject g in collection)
        {
            if (exception != null && g != exception)
            {
                g.SetActive(false);
            }
        }
        if (exception != null)
        {
            exception.SetActive(true);
        }
    }

    private void Start()
    {
        canv = transform.parent.GetComponentInParent<Canvas>();
        res = transform.parent.GetComponentInParent<UnitResources>();
        healthBar.maxValue = res.GetMaxHealth();
        updateValue(); 
    }

    public void updateValue()
    {
        if (healthBar != null)
        {
            healthBar.value = res.GetCurrentHealth();
            healthText.text = res.GetCurrentHealth().ToString();
            if (res.GetCurrentHealth() > res.GetMaxHealth())
            {
                healthText.color = shieldedHealthColor;
            }
            else
            {
                healthText.color = normalHealthColor;
            }
            playerColor.color = res.playerColor;
        }
    }

    public void OnEnable()
    {
        canv = transform.parent.GetComponentInParent<Canvas>();
        res = transform.parent.GetComponentInParent<UnitResources>();
        updateValue();
        res.OnValueChanged.AddListener(updateValue);
    }

    private void OnDisable()
    {
        canv = transform.parent.GetComponentInParent<Canvas>();
        res = transform.parent.GetComponentInParent<UnitResources>();
        if (res != null)
        {
            res.OnValueChanged.RemoveListener(updateValue);
        }
    }
}
