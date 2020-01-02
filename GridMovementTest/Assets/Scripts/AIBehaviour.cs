using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    

    public void PerformAITurn()
    {
        if (IsometricMetrics.state == BattleState.BATTLE)
        {
            foreach (Unit u in champion.team)
            {
                PerformAction(u);
            }
            EndTurnAI();
        }
    }

    void PerformAction(Unit u)
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
                    
        if(u.res.ActionPoints > 0)
        {
            //check if there is an enemy in a 3 tile range
            if (EnemiesInRange(u, 3) > 0) //if there is, check if you are already its neighbour
            {
                //Debug.Log("in range");
                Unit closest = ClosestEnemyUnit(u);
                if (closest.currentTile.IsTileNeighbour(u.currentTile)) //if you are, check what type of neighbour you are
                {
                    AttackIfPossible(u, closest);
                }
                else
                {
                    IsometricTile toMove = GenerateClosestPossibleTile(u); //move as close as possible to being a neighbour
                    CommitMoveUnit(u, toMove);
                    closest = ClosestEnemyUnit(u);
                    if (closest.currentTile.IsTileNeighbour(u.currentTile)) //if you become a neighbour
                    {
                        AttackIfPossible(u, closest);
                    }
                }
            }
            else //if there is not, check if you can move 2 tiles along the unit's tactic
            {
                if (canMoveThroughTactic(u)) //if you can, move and go onto the next unit
                {
                    //IsometricTile toMove = moveToRandomTile(u);
                    //CommitMoveUnit(u, toMove);

                    IsometricTile toMove = GenerateClosestPossibleTile(u);
                    CommitMoveUnit(u, toMove);
                }
                return; //if you cannot, go onto the next unit
            }
        }
    }

    void AttackIfPossible(Unit u, Unit closest)
    {
        IsometricTile[] tiles1 = closest.currentTile.GetNeighbors();
        IsometricTile[] tiles2 = closest.currentTile.GetNeighbors(closest.facingDirection);
        IsometricTile[] newTiles = tiles1.Except(tiles2).ToArray();

        if (newTiles.Contains(u.currentTile)) //if you're not at the front of an enemy
        {
            u.SetAttackTile(closest.currentTile); //attack
            u.res.willAttack = true;
        }
    }


    Unit ClosestEnemyUnit(Unit u)
    {
        Unit unit = null;
        foreach (IsometricTile t in grid.tiles)
        {
            if (t && t.unit)
            {
                if (t.unit.champ.playerNumber != u.champ.playerNumber)
                {
                    unit = t.unit;
                }
                if (u.currentTile.GetNeighbors().Contains(t))
                {
                    if (t.unit.champ.playerNumber != u.champ.playerNumber)
                    {
                        return t.unit;
                    }
                }
            }
        }
        return unit;
    }

    int EnemiesInRange(Unit u, int range)
    {
        int count = 0;
        foreach (IsometricTile t in grid.GetTilesInRange(range, u.currentTile))
        {
            if (t.unit)
            {
                if (t.unit.champ.playerNumber != u.champ.playerNumber)
                {
                    count++;
                }
            }
            
        }
        return count;
    }

    bool canMoveThroughTactic(Unit u)
    {
        foreach (IsometricTile t in u.InitializeTactic())
        {
            if (t.CostToMove < 2)
            {
                return true;
            }
        }
        return false;
    }

    IsometricTile moveToRandomTile(Unit u)
    {
        IsometricTile t = null;
        List<IsometricTile> tiles = u.InitializeTactic();
        int rand = Random.Range(0, tiles.Count);

        while (t == null)
        {
            for (int i = 0; i < rand; i++)
            {
                if (tiles[i].CostToMove < 2)
                {
                    t = tiles[i];
                }
            }
            rand = Random.Range(0, tiles.Count);
        }
        return t;
    }

    void CommitMoveUnit(Unit u, IsometricTile toMove)
    {
        if (toMove)
        {
            u.GeneratePathTo(toMove.coordinates.X, toMove.coordinates.Y);
            if (u.CheckTileWithTactic())
            {
                u.res.MakeAction();
                u.currentTile.CostToMove = 1;
                u.currentTile.unit = null;
                u.currentTile = null;
                toMove.SetUnit(u);
                u.res.willMove = true;
            }
        }
    }

    public void EndTurnAI()
    {
        for (int i = 0; i < 2; i++)
        {
            manager.UpdateAllTactics();
        }

        foreach (Unit u in champion.team)
        {
            if (u.gameObject.activeSelf)
            {
                u.PerformTurn();
            }
        }

        //this is uber bad
        manager.teams[0].SwitchActivePlayer(manager.teams[1]);
        manager.SwitchTeams();
    }


    IsometricTile GenerateClosestPossibleTile(Unit unit)
    {
        IsometricTile target;
        //we have a target tile 
        if (ClosestEnemyUnit(unit))
        {
            target = ClosestEnemyUnit(unit).currentTile;
        }
        else
        {
            Debug.Log("hmm");
            return null;
        }
        //List<IsometricTile> tiles = InitializeTactic();
        //find out which of the tiles is our target exactly by looping through all and matching up coordinates
        

        //dictionaries for the distance that the path will take as well as saving the previous tile in the path
        Dictionary<IsometricTile, float> dist = new Dictionary<IsometricTile, float>();
        Dictionary<IsometricTile, IsometricTile> prev = new Dictionary<IsometricTile, IsometricTile>();

        //setup the list of tiles that have not been visited yet
        List<IsometricTile> unvisited = new List<IsometricTile>();
        //setup the source of the movement, the current tile of the selected unit
        IsometricTile source = unit.currentTile;

        dist[source] = 0; //distance from source to destination starts at 0 
        prev[source] = null; //previous node is non-existant

        //Initialize distance, previous tiles and all the unvisited tiles
        foreach (IsometricTile t in grid.tiles)
        {
            if (t != source)
            {
                dist[t] = Mathf.Infinity;
                prev[t] = null;
            }
            unvisited.Add(t);
        }

        //start visiting all the tiles
        while (unvisited.Count > 0)
        {
            //u is the unvisited tile with the smallest distance
            IsometricTile u = null;
            foreach (IsometricTile possibleU in unvisited)
            {
                //if the distance between the tile that is being checked and the source 
                //is smaller than the current smallest distance tile and the source then 
                //the new smallest distance is the tile that is being checked
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            //if the target is found exit the while loop
            if (u.coordinates.X == target.coordinates.X && u.coordinates.Y == target.coordinates.Y)
            {
                break;
            }

            //you've visited u, so remove it from the list
            unvisited.Remove(u);

            //go to the neighbouring tiles
            foreach (IsometricTile t in u.GetNeighbors())
            {
                //only if there is actually a neighbour (so, not in any bordering tiles)
                if (t)
                {
                    //calculate whether the distance between the current tile and the source
                    //is smaller than the distance between the desired tile and the source
                    float alt = dist[u] + t.CostToMove;
                    if (alt < dist[t])
                    {
                        dist[t] = alt;
                        prev[t] = u;
                    }
                }
            }
        }

        if (prev[target] == null) //no route to target
        {
            return null;
        }
        //initialize the path for the unit
        List<IsometricTile> currentPath = new List<IsometricTile>();

        IsometricTile curr = target;

        //if the target has been reached
        while (curr != null)
        {
            //construct the path for the unit
            currentPath.Add(curr);
            //move backwards from the target to the source
            curr = prev[curr];
        }

       
        foreach (IsometricTile tt in currentPath)
        {
            foreach (IsometricTile ttt in unit.InitializeTactic())
            {
                if (tt.coordinates.ToString() == ttt.coordinates.ToString() && ttt.CostToMove < 2)
                {
                    return ttt;
                }
            }
        }
        return moveToRandomTile(unit);
    }


    //    IsometricTile[] tiles1 = closest.currentTile.GetNeighbors();
    //    IsometricTile[] tiles2 = closest.currentTile.GetNeighbors(closest.facingDirection);
    //    IsometricTile[] newTiles;
    //    //for (int i = 0; i < tiles1.Length; i++)
    //    //{
    //    //    if (tiles1[i].coordinates.ToString() == tiles2[i].coordinates.ToString())
    //    //    {
    //    //        newTiles.Add(tiles1[i]);
    //    //    }
    //    //}
    //    newTiles = tiles1.Except(tiles2).ToArray();

    //    if (newTiles.Contains(u.currentTile)) //if its a back neighbour
    //    {
    //        u.SetAttackTile(closest.currentTile);
    //        u.res.willAttack = true;
    //    }

    //    foreach (IsometricTile tt in newTiles)
    //    {
    //        if (closest.currentTile.GetOppositeNeighbor(closest.facingDirection).coordinates.ToString() == tt.coordinates.ToString())
    //        {

    //        }

    //        if (u.currentTile.coordinates.ToString() == tt.coordinates.ToString())
    //        {

    //        }
    //    }
}
