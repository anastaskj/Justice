using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text endText;
    public Button nextBattle;
    public Button quit;

    //add unlocks and progression animation

    public void SetText(int playerWon)
    {
        if (playerWon == 1)
        {
            endText.text = "You have been defeated!";
            nextBattle.gameObject.SetActive(false);
        }
        else
        {
            endText.text = "You are victorious!";
            nextBattle.gameObject.SetActive(true);
        }

        if (IsometricMetrics.progress == ProgressState.Final)
        {
            endText.text = "You have won the tournament! \n Your people will prosper!";
            nextBattle.gameObject.SetActive(false);
        }

    }

   
}
