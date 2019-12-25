using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IsometricCoordinates 
{
    [SerializeField]
    private int x, y;

    public int X { get { return x;}}
    public int Y { get { return y;}}

    public IsometricCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static IsometricCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new IsometricCoordinates(x, y);
    }

    //use this to improve coordinates in the future

    //public static IsometricCoordinates FromPosition(Vector3 position)
    //{
    //    //float x = (position.x / 0.5f + position.y / 0.25f) / 2;
    //    //float y = (position.y / 0.25f - (position.x / 0.50f)) / 2;

    //    //float x = (position.x + position.y / 0.5f) / 2;
    //    //float y = (position.y / 0.5f - position.x) / 2;

    //   float x = position.x + position.y / 0.5f;
    //   float y = position.y / 0.5f - position.x;

    //    int iX = Mathf.Abs(Mathf.RoundToInt(x));
    //    int iY = Mathf.Abs(Mathf.RoundToInt(y));
    //    Debug.Log(iX + " " + iY);

    //    return new IsometricCoordinates(iX, iY);
    //}

    public override string ToString()
    {
        return (X).ToString() + Y.ToString();
    }

    public string ToStringGUI()
    {
        return "(" + (X).ToString() + ", " + Y.ToString() + ")";
    }

}
