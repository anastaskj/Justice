using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorMovement : MonoBehaviour
{
    public int currentCursor;
    public Texture2D normalCursor;
    public Texture2D moveCursor;
    public Texture2D attackCursor;
    public Texture2D backAttackCursor;
    public Texture2D abilityCursor;

    public Vector2 hotspot = Vector2.zero;
    
    void Start()
    {
        ChangeToNormalCursor();
    }
    
    public void ChangeToNormalCursor()
    {
        Cursor.SetCursor(normalCursor, hotspot, CursorMode.Auto);
        currentCursor = 0;
    }

    public void ChangeToMoveCursor()
    {
        Cursor.SetCursor(moveCursor, hotspot, CursorMode.Auto);
        currentCursor = 1;
    }

    public void ChangeToAttackCursor()
    {
        Cursor.SetCursor(attackCursor, hotspot, CursorMode.Auto);
        currentCursor = 2;
    }

    public void ChangeToBackAttackCursor()
    {
        Cursor.SetCursor(backAttackCursor, hotspot, CursorMode.Auto);
        currentCursor = 3;
    }

    public void ChangeToAbilityCursor()
    {
        Cursor.SetCursor(abilityCursor, hotspot, CursorMode.Auto);
        currentCursor = 4;
    }
}
