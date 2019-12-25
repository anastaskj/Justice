using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IsometricMetrics 
{
    public static Sprite[] sprites;
    public static Sprite[] highlightedSprites;
    public static Transform[] flora;

    public static float ISO_WIDTH = 1f;
    public static float ISO_HEIGHT = 0.5f;

    public static int maxRangeTactic = 4;
    public static int maxRangeAbility = 4;

    public static bool editorMode = true;

    public static int maxUnitsPerTeam = 5;
}

public enum BattleState
{
   SPAWN, SETUP, BATTLE, END 
}
