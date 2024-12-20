using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class DraggableLetter : DraggableObject
{
    public static event Action<string> onSelect;
    public override void OnClick()
    {
        base.OnClick();
        var text = GetComponentInChildren<TextMeshPro>();
        onSelect?.Invoke(text.text);
        text.sortingLayerID = SortingLayer.NameToID("Foreground 1");
    }

    public override void OnRelease()
    {
        base.OnRelease();
        var text = GetComponentInChildren<TextMeshPro>();
        text.sortingLayerID = SortingLayer.NameToID("Default 1");
    }
}
