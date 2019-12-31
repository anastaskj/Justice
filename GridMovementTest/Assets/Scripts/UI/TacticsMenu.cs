using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsMenu : MonoBehaviour
{
    public RectTransform listContent;
    public TacticsUIItem tacticUIprefab;
    public TacticsManager tacticsManager;
    public IsometricUnitManager unitManager;
    public IsometricGrid grid;

    private Tactics activeTactic;
    public Tactics ActiveTactic
    {
        get { return activeTactic; }
        set
        {
            activeTactic = value;
        }
    }

    public void Open()
    {
        if (unitManager.selectedUnit != null && IsometricMetrics.state == BattleState.SETUP)
        {
            FillList();
            gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }

        for (int i = 0; i < tacticsManager.availableTactics.Length; i++)
        {
            TacticsUIItem item = Instantiate(tacticUIprefab);
            item.menu = this;
            item.Sprite = tacticsManager.availableTactics[i].picture;
            item.transform.SetParent(listContent, false);
            item.tactic = tacticsManager.availableTactics[i];
        }
    }

    public void SetTactic()
    {
        grid.ShowTacticPattern(unitManager.selectedUnit.possibleTiles, false);
        tacticsManager.SetTactic(unitManager.selectedUnit, activeTactic);
        Close();
        unitManager.selectedUnit.InitializeTactic();
    }
}
