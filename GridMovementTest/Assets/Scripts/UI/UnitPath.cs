using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitPath : MonoBehaviour
{
    public UnitPathManager pathManager;

    private Unit unit;

    List<GameObject> pathVisual;

    public void CreatePath()
    {
        List<IsometricTile> path = unit.currentPath;
        pathVisual = new List<GameObject>();
        for (int i = 0; i < path.Count; i++)
        {
            if (i > 0) //starting from the second
            {
                IsometricDirection dir = (IsometricDirection)GetTileNeighbour(path[i - 1], path[i]);

                switch (dir)
                {
                    case IsometricDirection.N:
                        break;
                    case IsometricDirection.NW:
                        break;
                    case IsometricDirection.W:
                        break;
                    case IsometricDirection.SW:
                        break;
                    case IsometricDirection.S:
                        break;
                    case IsometricDirection.SE:
                        break;
                    case IsometricDirection.E:
                        break;
                    case IsometricDirection.NE:
                        break;
                }
            }
        }
    }

    public int GetTileNeighbour(IsometricTile tile1, IsometricTile tile2)
    {
        int count = 0;
        for (int i = count; i < 8; i++)
        {
            if(!tile1.GetNeighbors()[i] == tile2) //if you've found the neighbour
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;
    }

    
    
}
