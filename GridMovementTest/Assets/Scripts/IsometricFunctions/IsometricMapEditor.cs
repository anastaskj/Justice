using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class IsometricMapEditor : MonoBehaviour
{
    public Sprite[] sprites;
    public IsometricFeatureManager features;
    public IsometricGrid grid;
    public Canvas gridCanvas;

    int activeFloraLevel;
    bool applyFlora;
    bool applySprite;
    bool spawnUnit;
    int activeTerrainTypeIndex;
    int unitSpawner;
    int brushSize;

    
    private void Awake()
    {
        SetTerrainTypeIndex(0);
        activeFloraLevel = 0;
        IsometricMetrics.flora = features.floraPrefabs;

        IsometricMetrics.editorMode = true;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleEditInput();
        }
    }
    
    void HandleEditInput()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.one);
        if (hit.collider != null)
        {
            //not efficient at all, math a better way
            if (IsometricMetrics.editorMode)
            {
                EditTiles(hit.collider.gameObject.GetComponent<IsometricTile>());
            }
        }
    }

    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = index;
    }

    public void SetApplyFlora(bool toggle)
    {
        applyFlora = toggle;
    }

    public void SetFloraLevel(float level)
    {
        activeFloraLevel = (int)level;
    }

    public void SetApplySprite(bool toggle)
    {
        applySprite = toggle;
    }

    public void SetSpawnUnit(bool toggle)
    {
        spawnUnit = toggle;
    }

    public void ShowUI(bool visible)
    {
        grid.ShowUI(visible);
    }

    public void ShowGrid(bool visible)
    {
        gridCanvas.gameObject.SetActive(visible);
    }

    public void SetUnitSpawner(float value)
    {
        unitSpawner = (int)value;
    }
    
    public void SetBrushSize(float size)
    {
        brushSize = (int)size;
    }

    void EditTiles(IsometricTile center)
    {
        foreach (IsometricTile i in grid.GetTilesInRange(brushSize, center))
        {
            EditTile(i);
        }
    }

    void EditTile(IsometricTile tile)
    {
        if (tile)
        {
            if (activeTerrainTypeIndex >= 0 && applySprite)
            {
                tile.TerrainTypeIndex = activeTerrainTypeIndex;
            }
            if (applyFlora)
            {
                if (tile.UnitSpawner == 0)
                {
                    if (tile.HasDetail)
                    {
                        features.Clear(tile);
                    }

                    if (activeFloraLevel != 0)
                    {
                        tile.FloraLevel = activeFloraLevel;
                        features.AddFlora(tile);
                    }
                }
            }
            else if (spawnUnit)
            {
                if (tile.FloraLevel == 0) //don't allow tiles with details to be spawners
                {
                    SetUnitSpawner(unitSpawner);
                    tile.UnitSpawner = unitSpawner;
                }
            }
        }
    }
}
