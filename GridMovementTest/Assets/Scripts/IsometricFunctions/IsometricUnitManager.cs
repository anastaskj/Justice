using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class IsometricUnitManager : MonoBehaviour
{
    //temp
    public Champion[] teams;

    public IsometricGrid grid;

    public EndGame endCanvas;

    public AIBehaviour aiController;
    //move
    public Image turnColor;

    public UnitDetailsUI unitDetails;
    //
    
    public UnityEvent OnSelectedUnitChanged = new UnityEvent();
    public UnityEvent OnActionChanged = new UnityEvent();

    public CursorMovement cursor;

    public Unit selectedUnit;
    private int currentPlayer;

    IsometricDirection newUnitDirection;
    
    bool unitMoveActive = true;
    bool unitUseAbilityActive = false;

    public bool MoveActive { get { return unitMoveActive; } }
    public bool UseAbilityActive { get { return unitUseAbilityActive; } }


    


    private void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

    public void SetNewUnitDirection(float d)
    {
        if (selectedUnit && !IsometricMetrics.editorMode && selectedUnit.res.ActionPoints > 0)
        {
            newUnitDirection = (IsometricDirection)d;
            if (selectedUnit.facingDirection != newUnitDirection)
            {
                selectedUnit.ChangeDirection(newUnitDirection);
                selectedUnit.res.MakeAction();
                unitUseAbilityActive = false;
                unitMoveActive = false;
            }
        }
        OnActionChanged.Invoke();
    }

    public void UnitUseAbilityActive()
    {
        if (!selectedUnit.res.usedAbility)
        {
            unitUseAbilityActive = !unitUseAbilityActive;
            unitMoveActive = false;
            OnActionChanged.Invoke();
        }
    }

    public void UnitMoveActive()
    {
        if (selectedUnit && !selectedUnit.res.willMove)
        {
            unitMoveActive = !unitMoveActive;
            unitUseAbilityActive = false;
            grid.ShowTacticPattern(selectedUnit.InitializeTactic(), unitMoveActive);
        }
        OnActionChanged.Invoke();
    }

    private void Start()
    {
        IsometricMetrics.state = BattleState.SPAWN;
        currentPlayer = 1;
        //endCanvas.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (selectedUnit != null)
            {
                HandleUnitInput(2);
            }
            if (Input.GetMouseButtonDown(0) )
            {
                unitDetails.gameObject.SetActive(false);
                HandleUnitInput(0);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                unitDetails.gameObject.SetActive(false);
                HandleUnitInput(1);
            }
            if (unitUseAbilityActive)
            {
                cursor.ChangeToAbilityCursor();
            }
        }
        else
        {
            cursor.ChangeToNormalCursor();
        }
    }

    public void isCursorOnEnemy(IsometricTile t)
    {
        if (t.unit != null && t.unit.champ.playerNumber != selectedUnit.champ.playerNumber && selectedUnit.isAdjacentToEnemy(t) && selectedUnit.res.ActionPoints > 0)
        {
            if (selectedUnit.currentTile.coordinates.ToString() == t.GetOppositeNeighbor(t.unit.facingDirection).coordinates.ToString())
            {
                cursor.ChangeToBackAttackCursor();
            }
            else
            {
                cursor.ChangeToAttackCursor();
            }
        }
        else
        {
            if (cursor.currentCursor != 1 && !unitUseAbilityActive) //not movement cursor
            {
                cursor.ChangeToNormalCursor();
            }
        }
    }

    void HandleUnitInput(int handleState)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.one);
        if (hit.collider != null)
        {
            if (!IsometricMetrics.editorMode)
            {
                IsometricTile t = hit.collider.gameObject.GetComponent<IsometricTile>();
                if (handleState == 0)
                {
                    HandleUnit(t);
                }
                else if (handleState == 1)
                {
                    HandleDetails(t);
                }
                else
                {
                    isCursorOnEnemy(t);
                }
            }
        }
    }

    void HandleDetails(IsometricTile tile)
    {
        if (tile.unit != null)
        {
            ShowDetails(tile.unit);
        }
    }

    public void ShowDetails(Unit u)
    {
        unitDetails.gameObject.SetActive(true);
        unitDetails.SetupDetails(u);
    }

    void HandleUnit(IsometricTile tile)
    {
        if (IsometricMetrics.state == BattleState.SETUP)
        {
            if (tile.unit != null && tile.unit.CheckUnitTeam())
            {
                tile.unit.ResetSelectedUnit();
                foreach (IsometricTile t in grid.GetSpawnerTiles(tile.unit.champ.playerNumber-1))
                {
                    if (t.UnitSpawner == 1)
                    {
                        t.ChangeTileColor(Color.blue);
                    }
                    else if (t.UnitSpawner == 2)
                    {
                        t.ChangeTileColor(Color.red);
                    }
                }
            }
            if (tile.unit == null && selectedUnit != null)
            {
                if (tile.UnitSpawner == selectedUnit.champ.playerNumber)
                {
                    selectedUnit.transform.position = tile.transform.position;
                    selectedUnit.currentTile.CostToMove = 1;
                    selectedUnit.currentTile.unit = null;
                    tile.SetUnit(selectedUnit);
                }
            }
        }
        else if (IsometricMetrics.state == BattleState.BATTLE)
        {
            //if a unit has not been selected and you've clicked on a tile that has a unit
            if (tile.unit != null && tile.unit.CheckUnitTeam() && !tile.unit.res.willMove && !unitUseAbilityActive)
            {
                cursor.ChangeToMoveCursor();
                tile.unit.ResetSelectedUnit();
            }
            if (selectedUnit && selectedUnit.res.ActionPoints > 0)
            {
                if (unitUseAbilityActive && !selectedUnit.res.usedAbility)
                {
                    if (selectedUnit.res.activeAbility != null)
                    {
                        if (tile != null)
                        {
                            if (grid.isTileInRange(tile, selectedUnit.res.activeAbility.abilityRange, selectedUnit.currentTile))
                            {
                                selectedUnit.UseActiveAbility(tile);
                            }
                        }
                        unitUseAbilityActive = false;
                        cursor.ChangeToNormalCursor();
                    }
                }
                else if (selectedUnit.isAdjacentToEnemy(tile) && !selectedUnit.res.willAttack)
                {
                    cursor.ChangeToAttackCursor();
                    selectedUnit.SetAttackTile(tile);
                    selectedUnit.res.willAttack = true;
                }
                else if (unitMoveActive && !selectedUnit.res.willMove)
                {
                    grid.ShowTacticPattern(selectedUnit.InitializeTactic(), true);
                    if (tile && selectedUnit && tile.CostToMove <= 1 && tile.unit == null && !selectedUnit.res.willMove)
                    {
                        selectedUnit.GeneratePathTo(tile.coordinates.X, tile.coordinates.Y);
                        if (selectedUnit.CheckTileWithTactic())
                        {
                            selectedUnit.res.MakeAction();
                            grid.ShowTacticPattern(selectedUnit.possibleTiles, false);
                            selectedUnit.currentTile.CostToMove = 1;
                            selectedUnit.currentTile.unit = null;
                            grid.HighlightTiles(selectedUnit.currentPath, true);
                            selectedUnit.currentTile = null;
                            tile.SetUnit(selectedUnit);
                            selectedUnit.res.willMove = true;
                            cursor.ChangeToNormalCursor();
                        }
                    }
                }
            }
        }
        if (selectedUnit != null) //not here
        {
            OnSelectedUnitChanged.Invoke();
        }
    }

    void CarryOutTurn(Unit u)
    {
        u.PerformTurn();
    }

    public void BeginGame()
    {
        IsometricMetrics.editorMode = false;
        for (int i = 0; i < 2; i++)
        {
            spawnUnits(i);
        }
        
        turnColor.color = Color.blue;
        IsometricMetrics.state = BattleState.SETUP;
    }

    public void SpawnUnit(IsometricTile tile, int i, int j)
    {
        Unit instance = teams[i].team[j] = Instantiate<Unit>(teams[i].team[j]);
        instance.manager = this;
        instance.grid = grid;
        instance.champ = teams[i];

        instance.transform.localPosition = tile.transform.position;
        tile.SetUnit(instance);

        if (instance.champ.playerNumber == 1)
        {
            instance.ChangeDirection(IsometricDirection.E);
            instance.res.playerColor = Color.blue;
        }
        else
        {
            instance.ChangeDirection(IsometricDirection.W);
            instance.res.playerColor = Color.red;
        }

        instance.InitializeTactic();
    }

    void spawnUnits(int teamNumber)
    {
        int count = teams[teamNumber].team.Count;

        List<IsometricTile> spawnTiles = grid.GetSpawnerTiles(teamNumber);
        
        if (count > spawnTiles.Count)
        {
            count = spawnTiles.Count;
        }
        for (int i = 0; i < count; i++)
        {
            SpawnUnit(spawnTiles[i], teamNumber, i);
        }
    }

    public void EndGame(int playerWon)
    {
        if (!endCanvas.gameObject.activeSelf)
        {
            endCanvas.gameObject.SetActive(true);
            endCanvas.SetText(playerWon);
        }
    }

    int turnCount = 0;

    public void EndTurn()
    {
        StopAllCoroutines();
        StartCoroutine(EndTurnCoroutine());
        //teams[0].ResetAllUnits(); //remove the select indicators
        //UpdateAllTactics();

        //foreach (Unit u in teams[0].team)
        //{
        //    if (u.gameObject.activeSelf)
        //    {
        //        CarryOutTurn(u);
        //        grid.ShowTacticPattern(u.InitializeTactic(), false);
        //    }
        //}

        //selectedUnit = null; //no unit is selected yet

        ////this is uber bad
        //teams[0].SwitchActivePlayer(teams[1]);
        //SwitchTeams();
        //turnCount++;
        //grid.ClearSpawnerTiles();
        //IsometricMetrics.state = BattleState.BATTLE;

        //aiController.PerformAITurn();
    }

    IEnumerator EndTurnCoroutine()
    {
        teams[0].ResetAllUnits(); //remove the select indicators
        UpdateAllTactics();

        foreach (Unit u in teams[0].team)
        {
            if (u.gameObject.activeSelf)
            {
                CarryOutTurn(u);
                grid.ShowTacticPattern(u.InitializeTactic(), false);
            }
        }

        selectedUnit = null; //no unit is selected yet

        //this is uber bad
        teams[0].SwitchActivePlayer(teams[1]);
        SwitchTeams();
        turnCount++;
        grid.ClearSpawnerTiles();
        IsometricMetrics.state = BattleState.BATTLE;

       
        for (float t = 0f; t < 5; t += Time.deltaTime)
        {
            yield return null;
        }
        aiController.PerformAITurn();
    }




    //move this to some UI related script
    public void SwitchTeams()
    {
        if (turnColor.color == Color.blue)
        {
            turnColor.color = Color.red;
            currentPlayer = 2;
        }
        else
        {
            turnColor.color = Color.blue;
            currentPlayer = 1;
        }
    }

    public void UpdateAllTactics()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (Unit u in teams[i].team)
            {
                u.InitializeTactic();
            }
        }
    }


    //public void EndTurn()
    //{
    //    for (int i = 0; i < 2; i++)
    //    {
    //        teams[i].ResetAllUnits(); //remove the select indicators
    //        UpdateAllTactics();
    //    }

    //    foreach (Unit u in teams[currentPlayer - 1].team)
    //    {
    //        if (u.gameObject.activeSelf)
    //        {
    //            CarryOutTurn(u);
    //            grid.ShowTacticPattern(u.InitializeTactic(), false);
    //        }
    //    }

    //    selectedUnit = null; //no unit is selected yet

    //    //this is uber bad
    //    teams[0].SwitchActivePlayer(teams[1]);
    //    SwitchTeams();
    //    turnCount++;

    //    if (turnCount < 2)
    //    {
    //        grid.ClearSpawnerTiles();
    //    }
    //    else
    //    {
    //        grid.ClearSpawnerTiles();
    //        IsometricMetrics.state = BattleState.BATTLE;
    //    }
    //}
}
