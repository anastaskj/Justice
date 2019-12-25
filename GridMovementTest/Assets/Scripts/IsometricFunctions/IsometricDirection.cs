using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IsometricDirection
{
    N, NW, W, SW, S, SE, E, NE
}

public static class IsometricDirectionExtentions
{

    public static IsometricDirection Opposite(this IsometricDirection direction)
    {
        return (int)direction < 4 ? (direction + 4) : (direction - 4);
    }
}

