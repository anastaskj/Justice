using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticsUIItem : MonoBehaviour
{
    public TacticsMenu menu;
    public Tactics tactic;

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

    public void Select()
    {
        menu.ActiveTactic = tactic;
    }
}
