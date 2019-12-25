using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class IsometricGrid : MonoBehaviour
{
    public int size;
    public IsometricTile tilePrefab;
    public Text tileLabelPrefab;

    public Sprite[] tileSprites;

    public Sprite[] highlightedTileSprites;

    public IsometricTile[] tiles;
    Canvas gridCanvas;

    public GameObject[] panelsEditor;
    public GameObject[] panelsBattle;


    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
        gridCanvas = GetComponentInChildren<Canvas>();
        tiles = new IsometricTile[size * size];

        IsometricMetrics.sprites = tileSprites;
        IsometricMetrics.highlightedSprites = highlightedTileSprites;
        ShowUI(false);
        CreateMap();
    }
    
    public void SetEditorMode(bool set)
    {
        IsometricMetrics.editorMode = set;
        OpenPanels();
    }

    public void OpenPanels()
    {
        if (IsometricMetrics.editorMode)
        {
            foreach (GameObject i in panelsEditor)
            {
                i.SetActive(true);
            }
            foreach (GameObject i in panelsBattle)
            {
                i.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject i in panelsEditor)
            {
                i.SetActive(false);
            }
            foreach (GameObject i in panelsBattle)
            {
                i.SetActive(true);
            }
        }
    }

    public List<IsometricTile> GetSpawnerTiles(int teamIndex) 
    {
        List<IsometricTile> newTiles = new List<IsometricTile>();
        foreach (IsometricTile t in tiles)
        {
            if (t.UnitSpawner == teamIndex + 1) //team number is just the index
            {
                newTiles.Add(t);
            }
        }
        return newTiles;
    }

    public void ClearSpawnerTiles()
    {
        foreach (IsometricTile t in tiles)
        {
            if (t.UnitSpawner != 0) //team number is just the index
            {
                t.ChangeTileColor(Color.white);
            }
        }
    }

    public void CreateMap()
    {
        if (transform.childCount != 0)
        {
            ClearMap();
        }
        for (int x = 0, i = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                CreateTile(x, y, i++);
            }
        }
    }

    void ClearMap()
    {
        if (tiles.Length > 0)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    public bool isTileInRange(IsometricTile check, int range, IsometricTile center)
    {
        //HighlightTiles(GetTilesInRange(range, center), true);
        foreach (IsometricTile t in GetTilesInRange(range, center))
        {
            if (check.coordinates.X == t.coordinates.X && check.coordinates.Y == t.coordinates.Y)
            {
                return true;
            }
        }
        return false;
    }

    public List<IsometricTile> GetTilesInRange(int range, IsometricTile center)
    {
        List<IsometricTile> temp = new List<IsometricTile>();

        int centerX = center.coordinates.X;
        int centerY = center.coordinates.Y;
        for (int x = centerX - range; x <= centerX + range; x++) //get the x+ tiles within the range
        {
            for (int y = centerY - range; y <= centerY + range; y++) //get the y+ tiles within the range
            {
                temp.Add(GetTile(new IsometricCoordinates(x, y)));
            }
        }
        return temp;
    }

    public IsometricTile GetTile(IsometricCoordinates coordinates)
    {
        int x = coordinates.X;
        int y = coordinates.Y;
        int res = Mathf.Abs((x * size) + y);
        if (res < size*size)
        {
            return tiles[Mathf.Abs((x * size) + y)];
        }
        return null;
    }

    //x-1, y+0.5
    void CreateTile(int x, int y, int i) //create an isometric tile that has coordinates corresponding to its posiion
    {
        Vector3 position;
        position.x = x + y; //x position moves by 1 unit to build the tile up
        position.y = y * 0.5f - x * 0.5f; //formula for isometric y movement
        position.z = 0; //no rotation

        //instantiate and add to array of tiles
        IsometricTile tile = tiles[i] = Instantiate<IsometricTile>(tilePrefab);
        tile.transform.SetParent(transform, false);
        tile.transform.localPosition = position;
        tile.coordinates = IsometricCoordinates.FromOffsetCoordinates(x, y); //initilize tile coordinates

        tile.grid = this;

        //set neighbours
        if (y>0) //ignore starting row, which has no S neighbour
        {
            tile.SetNeighbor(IsometricDirection.S, tiles[i - 1]);
        }
        if (x>0) //ignore starting column which has no W neighbour
        {
            tile.SetNeighbor(IsometricDirection.W, tiles[i - size]);
            if (y>0 && y<size-1) //ignore bordering rows that do not have SW or NW borders
            {
                tile.SetNeighbor(IsometricDirection.SW, tiles[i - (size + 1)]);
                tile.SetNeighbor(IsometricDirection.NW, tiles[i - (size - 1)]);
            }
            else if (y == 0) //only use the starting row so you can set the NW neighbours
            {
                tile.SetNeighbor(IsometricDirection.NW, tiles[i - (size - 1)]);
            }
            else if (y == size - 1) //only use the end row so that you can set the SW neighbours
            {
                tile.SetNeighbor(IsometricDirection.SW, tiles[i - (size + 1)]);
            }
        }

        //add coordinate labels
        Text label = Instantiate<Text>(tileLabelPrefab); 
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.y);
        label.text = tile.coordinates.ToString();
    }

    public void ShowUI(bool visible)
    {
        gridCanvas.gameObject.SetActive(visible);
    }
    
    public void HighlightTiles(List<IsometricTile> path, bool set)
    {
        foreach (IsometricTile t in path)
        {
            t.Highlighted = set;
        }
    }

    //awful, change
    public void ShowTacticPattern(List<IsometricTile> possibleTiles, bool set)
    {
        foreach (IsometricTile t in possibleTiles)
        {
            if (set)
            {
                t.ChangeTileColor(Color.gray);
            }
            else
            {
                t.ChangeTileColor(Color.white);
            }
        }
    }

    public void Save(BinaryWriter writer)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].Save(writer);
        }
    }

    public void Load(BinaryReader reader, IsometricFeatureManager floraMaker, IsometricUnitManager unitManager)
    {
        CreateMap();
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].Load(reader);
            if (tiles[i].FloraLevel > 0)
            {
                floraMaker.Clear(tiles[i]);
                floraMaker.AddFlora(tiles[i]);
            }
            else if (!tiles[i].HasDetail && tiles[i].transform.childCount > 1)
            {
                floraMaker.Clear(tiles[i]);
            }
            tiles[i].ChangeTileColor(Color.white);
        }
    }
}
