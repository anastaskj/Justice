using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FactionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float timer;
    [SerializeField] Image virulentBG;
    [SerializeField] Image kingdomButton;
    [SerializeField] Image virulentButton;
    [SerializeField] Text virulentText;
    [SerializeField] Text kingdomText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.gameObject.tag == "VirulentMenu")
        {
            TransitionToVirulent();
        }
        else
        {
            TransitionToKingdom();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    void TransitionToVirulent()
    {
        Color c = new Color(255, 255, 255, 1);
        virulentBG.color = c;
        c = new Color(255, 255, 255, 0.5f);
        kingdomButton.color = c;
        kingdomText.color = c;
        c = new Color(255, 255, 255, 1);
        virulentButton.color = c;
        virulentText.color = c;
    }

    void TransitionToKingdom()
    {
        Color c = new Color(0, 0, 0, 0);
        virulentBG.color = c;
        c = new Color(255, 255, 255, 0.5f);
        virulentButton.color = c;
        virulentText.color = c;
        c = new Color(255, 255, 255, 1);
        kingdomButton.color = c;
        kingdomText.color = c;
    }
}
