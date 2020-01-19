using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu"); 
    }

    public void GoToEditor()
    {
        SceneManager.LoadScene("EditorScene");
    }

    public void GoToAIBattle()
    {
        SceneManager.LoadScene("BattleAIScene");
    }
}
