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
    }
}
