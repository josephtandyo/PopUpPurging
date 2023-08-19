using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class is for mouse cursor changing on clickable buttons by creating a custom event
/// </summary>
public class CustomEvents : EventTrigger
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        // call the mouse control script clickable mouse change
        MouseControlScript.instance.Clickable();
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        // call the mouse control script default mouse change
        MouseControlScript.instance.Default();
    }
}
