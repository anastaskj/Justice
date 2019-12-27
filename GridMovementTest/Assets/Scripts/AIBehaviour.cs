using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    Champion champion;
    [SerializeField]
    IsometricGrid grid;
    [SerializeField]
    IsometricUnitManager manager;

    public void SetupUnits()
    {
        //Unit u;
        //IsometricTile spawnerTile;
        //if (manager.state == BattleState.SETUP)
        //{
        //    for (int i = 0; i < champion.team.Count; i++)
        //    {
        //        u = champion.team[i];
        //        if (true)
        //        {

        //        }
        //    }
        //}
    }

    int CheckUnitProximity(Unit u)
    {
        return 1;
    }

    public void PerformAction()
    {
        
        //check if there is an enemy in a 3 tile range
        //if there is not, check if you can move 2 tiles along the unit's tactic
            //if you can, move and go onto the next unit
            //if you cannot, go onto the next unit
        //if there is, check if you are already its neighbour
            //if you are, check what type of neighbour you are:

                //if you are a front neighbour
                    //check if you can move to the back
                        //if you can, move and attack enemy
                        //if you cannot, check if you can move to the sides
                            //if you can, move and attack enemy
                            //if you cannot, go onto the next unit
                //if you are a side neighbour
                    //check if you can move to the back
                        //if you can, move and attack enemy
                        //if you cannot, attack enemy
                //if you are a back neighbour
                    //attack enemy

             //if you are not a neighbour already:
                    
                //check if your tactic will allow you to move so you can become a neighbour
                    //if you can, check if you can move to the back
                        //if you can, move and attack enemy
                        //if you cannot, check if you can move to the sides
                            //if you can, move and attack enemy
                            //if you cannot, go onto the next unit
                    //if you cannot, go onto the next unit
    }

}
