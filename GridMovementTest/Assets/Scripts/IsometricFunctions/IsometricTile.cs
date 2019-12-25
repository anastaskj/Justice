using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IsometricTile : MonoBehaviour
{
    public IsometricCoordinates coordinates;
    public IsometricGrid grid;
    public Unit unit;

    [SerializeField]
    float costToMove = 1;
    [SerializeField]
    bool hasDetail = false;
    [SerializeField]
    int floraLevel;
    [SerializeField]
    int terrainTypeIndex;
    [SerializeField]
    IsometricTile[] neighbors;

    [SerializeField]
    int unitSpawner = 0;

    GameObject attackedIndicator;

    public bool highlighted = false;


    private void Start()
    {
        attackedIndicator = transform.GetChild(0).gameObject;
    }
    public void EnableAttackedIndicator(bool toggle)
    {
        attackedIndicator.SetActive(toggle);
    }

    public IsometricTile[] GetNeighbors()
    {
        return neighbors;
    }

    public IsometricTile[] GetNeighbors(IsometricDirection dir)
    {
        IsometricTile[] dirNeigh = new IsometricTile[3];
        //we want the 3 neighbours in a certain direction
        //so N would give us the tiles at N, NE, NW
        if ((int)dir == 0) //north has to loop back to NE
        {
            dirNeigh[0] = neighbors[7];
            dirNeigh[1] = neighbors[0];
            dirNeigh[2] = neighbors[1];
        }
        else
        {
            for (int i = -1; i < 2; i++)
            {
                dirNeigh[i+1] = neighbors[(int)dir + i];
            }
            
        }

        return dirNeigh;
    }

    public IsometricTile GetOppositeNeighbor(IsometricDirection dir)
    {
        return neighbors[(int)dir.Opposite()];
    }

    public bool IsTileNeighbour(IsometricTile t)
    {
        foreach (IsometricTile tile in neighbors)
        {
            if (t != null && tile != null)
            {
                if (t.coordinates.X == tile.coordinates.X && t.coordinates.Y == tile.coordinates.Y)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void SetNeighbor(IsometricDirection direction, IsometricTile tile)
    {
        neighbors[(int)direction] = tile;
        tile.neighbors[(int)direction.Opposite()] = this;
    }


    public bool HasDetail
    {
        get
        {
            return hasDetail;
        }
        set
        {
            if (hasDetail != value)
            {
                hasDetail = value;
            }
        }
    }

    public int UnitSpawner
    {
        get
        {
            return unitSpawner;
        }
        set
        {
            if (unitSpawner != value)
            {
                unitSpawner = value;
                setSpawner();
            }
        }
    }

    public int FloraLevel
    {
        get
        {
            return floraLevel;
        }
        set
        {
            if (floraLevel != value)
            {
                floraLevel = value;
            }
        }
    }

    public void SetSprite()
    {
        if (!highlighted)
            GetComponentInParent<SpriteRenderer>().sprite = IsometricMetrics.sprites[terrainTypeIndex];
        else
            GetComponentInParent<SpriteRenderer>().sprite = IsometricMetrics.highlightedSprites[terrainTypeIndex];
    }

    public Sprite Sprite
    {
        get
        {
            if (!highlighted)
            {
                return IsometricMetrics.sprites[terrainTypeIndex];
            }
            else
            {
                return IsometricMetrics.highlightedSprites[terrainTypeIndex];
            }
        }
    }

    public bool Highlighted
    {
        get
        {
            return highlighted;
        }
        set
        {
            if (highlighted != value)
            {
                highlighted = value;
                SetSprite();
            }
        }
    }

    public void setSpawner()
    {
        if (unitSpawner == 0)
        {
            ChangeTileColor(Color.white);
        }
        else if (unitSpawner == 1)
        {
            ChangeTileColor(Color.blue);
        }
        else if (unitSpawner == 2)
        {
            ChangeTileColor(Color.red);
        }
      
    }

    public void ChangeTileColor(Color c)
    {
        //change to set when starting
        GetComponentInParent<SpriteRenderer>().color = c;
    }

    public float CostToMove
    {
        get
        {
            return costToMove;
        }
        set
        {
            if (costToMove != value)
            {
                costToMove = value;
            }
        }
    }

    public int TerrainTypeIndex
    {
        get
        {
            return terrainTypeIndex;
        }
        set
        {
            if (terrainTypeIndex != value)
            {
                terrainTypeIndex = value;
                SetSprite();
            }
        }
    }

    public float DistanceTo(IsometricTile t)
    {
        Debug.Log(t.coordinates.X + " " + t.coordinates.Y);
        return Vector2.Distance(new Vector2(this.coordinates.X, this.coordinates.Y), new Vector2(t.coordinates.X, t.coordinates.Y));
    }

    public void SetUnit(Unit u)
    {
        unit = u;
        u.currentTile = this;

        CostToMove = 10;
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write((byte)terrainTypeIndex);
        writer.Write((byte)floraLevel);
        writer.Write(hasDetail);
        writer.Write((byte)CostToMove);
        writer.Write((byte)unitSpawner);
    }

    public void Load(BinaryReader reader)
    {
        TerrainTypeIndex = reader.ReadByte();
        FloraLevel = reader.ReadByte();
        HasDetail = reader.ReadBoolean();
        CostToMove = reader.ReadByte();
        unitSpawner = reader.ReadByte();
    }
}
