using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tactic", menuName = "Tactics")]
public class Tactics : ScriptableObject
{
    public int flexibilityRequirement;
    public TacticCoordinate[] tiles;

    public Sprite picture;


    public TacticCoordinate[] GetDirectionTactics(IsometricDirection facing)
    {
        TacticCoordinate[] newTiles = new TacticCoordinate[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            if (facing == IsometricDirection.E)
            {
                newTiles = tiles;
            }
            else if (facing == IsometricDirection.W)
            {
                newTiles[i].X = -tiles[i].X;
                newTiles[i].Y = tiles[i].Y;
            }
            else if (facing == IsometricDirection.N)
            {
                newTiles[i].X = tiles[i].Y;
                newTiles[i].Y = tiles[i].X;
            }
            else if (facing == IsometricDirection.S)
            {
                newTiles[i].X = tiles[i].Y;
                newTiles[i].Y = -tiles[i].X;
            }
        }
        return newTiles;
    }
}
