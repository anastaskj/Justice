using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricFeatureManager : MonoBehaviour
{
    public Transform[] floraPrefabs;

    public void Clear(IsometricTile tile)
    {
        if (tile.transform.childCount > 1)
        {
            Transform detail = tile.transform.GetChild(1);
            if (detail)
            {
                Destroy(detail.gameObject);
                tile.HasDetail = false;
                tile.FloraLevel = 0;
                tile.CostToMove = 1;
            }
        }
    }

    public void AddFlora(IsometricTile tile)
    {
        Transform instance = Instantiate(floraPrefabs[tile.FloraLevel-1], tile.gameObject.transform, false);
        tile.HasDetail = true;
        tile.CostToMove = 2;
    }
}
