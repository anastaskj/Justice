using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionManager : MonoBehaviour
{
    //champions are ordered by their army level from 3 to 5 units

    public Champion[] kingdomPlayerChampions;
    public Champion[] virulentPlayerChampions;

    public Champion[] kingdomEnemyChampions;
    public Champion[] virulentEnemyChampions;

    public IsometricUnitManager unitmanager;

    public void InstantiateChampions(ProgressState currentProgress, Factions chosenFaction)
    {
        Champion player = null;
        switch (chosenFaction)
        {
            case Factions.Kingdom:
                player = Instantiate(kingdomPlayerChampions[(int)currentProgress]);
                break;
            case Factions.Virulent:
                player = Instantiate(virulentPlayerChampions[(int)currentProgress]);
                break;

            default:
                break;
        }

        Champion enemy = null;
        int rand = Random.Range(1, 3);

        if (rand == 1)
        {
            enemy = Instantiate(kingdomEnemyChampions[(int)currentProgress]);
        }
        else
        {
            enemy = Instantiate(virulentEnemyChampions[(int)currentProgress]);
        }

        if (player && enemy)
        {
            unitmanager.teams[0] = player;
            unitmanager.teams[1] = enemy;
            unitmanager.aiController.champion = enemy;
        }
    }
}
