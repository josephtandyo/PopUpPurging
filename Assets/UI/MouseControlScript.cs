using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for changing the cursor image from clickable to default and vice versa
/// </summary>
public class MouseControlScript : MonoBehaviour
{
    public static MouseControlScript instance;

    // the two states of the mouse cursor
    public Texture2D DefaultCursor, ClickableCursor;
    
    // allows only one instance from this class. Makes it accessible from any class
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);   
    }

    // start with the default cursor
    private void Start()
    {
        Default();
    }

    // changes the cursor to the clickable cursor
    public void Clickable()
    {
        Cursor.SetCursor(ClickableCursor, Vector2.zero, CursorMode.Auto);
    }

    // changes the cursor to the default cursor
    public void Default()
    {
        Cursor.SetCursor(DefaultCursor, Vector2.zero, CursorMode.Auto);
    }
}