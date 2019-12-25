using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour
{
    public List<Unit> team;

    public int playerNumber;
    public bool isActivePlayer;
    public int teamSize;

    private void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        teamSize = team.Count;
    }

    public void SwitchActivePlayer(Champion opponent)
    {
        if (isActivePlayer)
        {
            opponent.isActivePlayer = true; //switch active player things
            isActivePlayer = false;
        }
        else
        {
            isActivePlayer = true;
            opponent.isActivePlayer = false; //switch active player things
        }
    }

    public void ResetAllUnits()
    {
        foreach (Unit u in team)
        {
            u.SetActiveIndicator(false);
        }
    }
}
