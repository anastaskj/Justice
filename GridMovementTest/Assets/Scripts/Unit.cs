using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;
using System.IO;

public class Unit : MonoBehaviour
{
    public IsometricTile currentTile;
    public List<IsometricTile> currentPath = null;
    public List<IsometricTile> possibleTiles = null;

    public UnitResources res;
    public UnitResourceUI resUI;
    
    //maybe move these 2 as well later
    public IsometricDirection facingDirection;
    public Tactics tactic;

    public Champion champ;

    public Sprite sprite;
    
    public IsometricGrid grid;
    public IsometricUnitManager manager;
    
    GameObject unitIndicator;
    
    public IsometricTile attackTile;

    public List<UnitAbility> abilities;

    Animator anim;

    [SerializeField] AudioManager audio;
    
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        res.ActionPoints = 3;
        unitIndicator = transform.GetChild(0).gameObject;
        triggerAbility(AbilityTrigger.ON_START_GAME);
        res.hasTakenDamage = false;
        res.usedAbility = false;
    }

    public void ResetSelectedUnit()
    {
        manager.selectedUnit = this;
        SetActiveIndicator(true);

        if (champ.playerNumber == 1)
        {
            unitIndicator.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            unitIndicator.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }

        foreach (Unit u in champ.team)
        {
            if (u != this)
            {
                u.SetActiveIndicator(false);
                if (IsometricMetrics.state == BattleState.BATTLE)
                {
                    grid.ShowTacticPattern(u.possibleTiles, false);
                }
            }
        }
        manager.OnSelectedUnitChanged.Invoke();
    }

    public void SetActiveIndicator(bool toggle)
    {
        if (unitIndicator != null)
        {
            unitIndicator.SetActive(toggle);
        }
    }

    public void TakeDamage(int damage)
    {
        resUI.ShowDamageLabel(damage);
        res.TakeDamage(damage);
        if (res.GetCurrentHealth() <= 0)
        {
            champ.teamSize -= 1;
            currentTile.CostToMove = 1;
            currentTile.unit = null;
            StartCoroutine(DeathAnimation());
        }
    }
    
    public void RegainHealth(int health)
    {
        if (res.GetCurrentHealth() != res.GetMaxHealth())
        {
            resUI.ShowHealthLabel(health);
            res.RegainHealth(health);
        }
    }


    public void MoveToTile()
    {
        if (!res.hasMoved)
        {
            IsometricTile end = GetEndOfPath();
            if (end != null)
            {
                //transform.position = end.transform.position;
                StartMovement(TravelPath(currentPath));

                grid.HighlightTiles(currentPath, false);
                currentPath = null;
                res.hasMoved = true;
                InitializeTactic();
            }
        }
    }


    public IsometricTile GetEndOfPath()
    {
        if (currentPath == null)
        {
            return null;
        }

        return currentPath[currentPath.Count - 1];
    }

    public void ChangeDirection(IsometricDirection newDir) //change 
    {
        if (newDir != facingDirection /*&& !hasChangeDir*/)
        {
            facingDirection = newDir;
            res.hasChangeDir = true;

            resUI.ShowDirectionArrow(newDir);
            grid.ShowTacticPattern(possibleTiles, false);
            InitializeTactic();
        }
    }

    public void SetAttackTile(IsometricTile t)
    {
        if (!res.hasAttacked)
        {
            if (t != null)
            {
                attackTile = t;
                
                attackTile.EnableAttackedIndicator(true);
            }
            res.hasAttacked = true;
            res.MakeAction();
        }
    }

    public void AttackTile()
    {
        if (attackTile != null)
        {
            if (attackTile.unit != null)
            {
                Unit attackedUnit = attackTile.unit;
                if (currentTile.IsTileNeighbour(attackTile) && attackedUnit.champ != this.champ)
                {
                    CalculateDirectionDamage(attackedUnit);

                    attackTile.unit.triggerAbility(AbilityTrigger.ON_DEFENSE);

                    attackedUnit.TakeDamage(attackedUnit.res.damageToBeTaken);
                    ChangeFacingDirectionAttack(attackedUnit);
                    attackTile.EnableAttackedIndicator(false);

                    triggerAbility(AbilityTrigger.ON_ATTACK);

                    anim.SetBool("AttackRight", false);
                    anim.SetBool("AttackLeft", false);

                    //disable the attack tile
                }
            }
        }
    }

    private void CalculateDirectionDamage(Unit attackedUnit)
    {
        int damage = -1;
        IsometricDirection attackedUnitFacing = attackedUnit.facingDirection;

        foreach (IsometricTile t in attackedUnit.currentTile.GetNeighbors(attackedUnitFacing))
        {
            if (t != null)
            {
                if (currentTile.coordinates.X == t.coordinates.X && currentTile.coordinates.Y == t.coordinates.Y)
                {
                    damage = 0;
                    break;
                }
            }
        }

        if (damage < 0) //if the tile of the attacker is not in front of the defender
        {
            if (currentTile == attackedUnit.currentTile.GetOppositeNeighbor(attackedUnitFacing)) //the opposite of the facing direction of the defender
            {
                damage = res.BackDamage;
            }
            else //sides
            {
                damage = res.SideDamage;
            }
        }
        attackedUnit.res.damageToBeTaken = damage;
    }

    public void ResetTurn() //move
    {
        res.hasMoved = false;
        res.hasChangeDir = false;
        res.hasAttacked = false;

        res.willAttack = false;
        res.willMove = false;
        res.ActionPoints = 2;
        res.hasTakenDamage = false;
    }

    public bool CheckUnitTeam()
    {
        if (champ.isActivePlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isAdjacentToEnemy(IsometricTile tile)
    {
        foreach (IsometricTile t in currentTile.GetNeighbors())
        {
            if (t == tile)
            {
                if (t.unit != null && t.unit.champ.playerNumber != champ.playerNumber)
                {
                    return true;
                }
            }
        }
        return false;
    }
   

    void ChangeFacingDirectionAttack(Unit attackedUnit)
    {
        //get the tile that you are attacking from
        //get the dir neighbours from that tile and the facing direction of the unit that is being attacked

        IsometricTile t = currentTile;
        IsometricTile[] tiles = attackedUnit.currentTile.GetNeighbors();
        IsometricDirection direction = IsometricDirection.N;

        for (int i = 0; i < tiles.Length; i++)
        {
            // 0-7 index to represent the facing direction 
            if (tiles[i] == currentTile) //if the neighbouring tile matches the attacking unit's tile
            {
                direction = (IsometricDirection)i;

                if ((int)direction%2 == 1) //if its a corner direction
                {
                    if ((int)direction == 1)
                    {
                        direction = IsometricDirection.W;
                    }
                    else
                    {
                        direction -= 1;
                    }
                }
                break;
            }
        }
        this.ChangeDirection(direction.Opposite());
        audio.MoveSound();
    }
   


    public List<IsometricTile> InitializeTactic()
    {
        //when you deselect tiles, diselect every one from the tactic to make sure
        if (tactic != null)
        {
            List<IsometricTile> usableTiles = new List<IsometricTile>();
            for (int i = 0; i < grid.tiles.Length; i++) //think of a better way   grid.GetTilesInRange(IsometricMetrics.maxRangeTactic, this.currentTile).Count
            {
                foreach (TacticCoordinate tc in tactic.GetDirectionTactics(facingDirection))
                {
                    if (currentTile.coordinates.X + tc.X == grid.tiles[i].coordinates.X && currentTile.coordinates.Y + tc.Y == grid.tiles[i].coordinates.Y)
                    {
                        usableTiles.Add(grid.tiles[i]);
                    }
                }
            }
            possibleTiles = usableTiles;
            List<IsometricTile> newUsableTiles = new List<IsometricTile>();
            for (int i = 0; i < usableTiles.Count; i++)
            {
                if (usableTiles[i].CostToMove < 2 && usableTiles[i].FloraLevel < 1 && usableTiles[i].unit == null)
                {
                    newUsableTiles.Add(usableTiles[i]);
                }
            }
            return newUsableTiles;
        }
        return null;
    }

    //Dijkstra's algorithm for pathfinding
    public void GeneratePathTo(int x, int y)
    {
        //the selected unit has a path that needs to be cleared every time it wants to move again
        currentPath = null;

        //we have a target tile 
        IsometricTile target = null;
        //List<IsometricTile> tiles = InitializeTactic();
        //find out which of the tiles is our target exactly by looping through all and matching up coordinates

        foreach (IsometricTile t in grid.tiles)
        {
            if (t.coordinates.X == x && t.coordinates.Y == y && t.CostToMove <= 1)
            {
                target = t;
            }
        }

        //dictionaries for the distance that the path will take as well as saving the previous tile in the path
        Dictionary<IsometricTile, float> dist = new Dictionary<IsometricTile, float>();
        Dictionary<IsometricTile, IsometricTile> prev = new Dictionary<IsometricTile, IsometricTile>();

        //setup the list of tiles that have not been visited yet
        List<IsometricTile> unvisited = new List<IsometricTile>();
        //setup the source of the movement, the current tile of the selected unit
        IsometricTile source = currentTile;

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
            return;
        }
        //initialize the path for the unit
        currentPath = new List<IsometricTile>();

        IsometricTile curr = target;

        //if the target has been reached
        while (curr != null)
        {
            //construct the path for the unit
            currentPath.Add(curr);
            //move backwards from the target to the source
            curr = prev[curr];
        }

        //reverse the path so you start at the source
        currentPath.Reverse();
        // MakeAction("move");

        // currentTile.CostToMove = 1;
        // currentTile.unit = null;
       
    }

    public bool CheckTileWithTactic()
    {
        foreach (IsometricTile t in possibleTiles)
        {
            if (t == GetEndOfPath())
            {
                t.CostToMove = int.MaxValue;
                return true;
            }
        }
        return false;
    }

    public IEnumerator TravelPath(List<IsometricTile> path)
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 a = path[i - 1].transform.position;
            Vector3 b = path[i].transform.position;

            if (a.x > b.x)
            {
                anim.SetBool("MoveLeft", true);
                anim.SetBool("MoveRight", false);
            }
            else
            {
                anim.SetBool("MoveRight", true);
                anim.SetBool("MoveLeft", false);
            }
            audio.MoveSound();
            for (float t = 0f; t < 1f; t += Time.deltaTime * res.speed)
            {
                transform.localPosition = Vector3.Lerp(a, b, t);
                yield return null;
            }
        }
       
        if (res.willAttack)
        {
            StartAttack();
        }
        anim.SetBool("MoveRight", false);
        anim.SetBool("MoveLeft", false);
        anim.SetBool("Idle", true);

        ResetTurn();
    }

    public IEnumerator PerformAttack()
    {
        if (attackTile.transform.position.x > currentTile.transform.position.x)
        {
            anim.SetTrigger("AttackRight");
        }
        else
        {
            anim.SetTrigger("AttackLeft");
        }
        audio.AttackSound();
        for (float t = 0f; t < res.attackTimer; t += Time.deltaTime)
        {
            yield return null;
        }
        AttackTile();
        anim.SetBool("Idle", true);
        ResetTurn();
    }

    IEnumerator DeathAnimation()
    {
        audio.DeathSound();
        anim.SetTrigger("Death");
        for (float t = 0f; t < res.deathTimer; t += Time.deltaTime)
        {
            yield return null;
        }
        gameObject.SetActive(false);

        if (champ.teamSize == 0)
        {
            manager.EndGame(champ.playerNumber);
        }
    }

    public void PerformTurn()
    {
        if (res.willMove)
        {
            MoveToTile();
        }
        else
        {
            if (res.willAttack)
            {
                StartAttack();
            }
        }
        triggerAbility(AbilityTrigger.ON_END_TURN);
    }

    public void StartMovement(IEnumerator coroutine) 
    {
        StopAllCoroutines();
        anim.SetBool("Idle", false);
        StartCoroutine(coroutine);
    }

    public void StartAttack()
    {
        StopAllCoroutines();
        StartCoroutine(PerformAttack());
    }
    
    public void triggerAbility(AbilityTrigger trigger)
    {
        if (gameObject.activeSelf)
        {
            foreach (UnitAbility a in abilities)
            {
                if (a.trigger == trigger)
                {
                    a.ExecuteAbility(this);
                }
            }
        }
    }

    public void UseActiveAbility(IsometricTile target) //move
    {
        if (grid.isTileInRange(target, res.activeAbility.abilityRange, currentTile))
        {
            audio.AbilitySound();
            res.activeAbility.ExecuteAbility(this, target);
            res.MakeAction();
            res.usedAbility = true;
        }
    }
}
