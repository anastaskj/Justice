using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TacticCoordinate
{
    [SerializeField]
    private int x, y;

    public int X { get { return x; } set { x = value; } }
    public int Y { get { return y; } set { y = value; } }

    public TacticCoordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
