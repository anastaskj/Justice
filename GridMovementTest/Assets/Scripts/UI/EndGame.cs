using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text endText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void SetText(int playerWon)
    {
        if (playerWon == 1)
        {
            endText.text += "Red player has won";
        }
        else
        {
            endText.text += "Blue player has won";
        }
    }
}
