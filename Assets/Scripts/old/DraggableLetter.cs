using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class DraggableLetter : VisualElement
{
    public static event Action<string> onSelect;
    public static event Action<DraggableLetter> onDragEnd;
    //public WritingLine line;
    public Vector2 originalPos;
    public string value;
    public bool isDragging = false;
    private string draggableLetterClassName = "draggable-letter";
    private VisualElement bodyEl;

    public DraggableLetter(string letter, VisualElement mainEl)
    {
        value = letter;
        AddToClassList(draggableLetterClassName);

        var letterLabel = new Label(letter);
        Add(letterLabel);
        this.bodyEl = mainEl;

        RegisterCallback<PointerDownEvent>(OnDragStart);
        RegisterCallback<PointerMoveEvent>(OnDrag);
        RegisterCallback<PointerUpEvent>(OnDragEnd);
    }

    public void Select()
    {
        onSelect?.Invoke(value);
        CalculatePos();
    }
    private void CalculatePos()
    {
        Vector2 pos = worldTransform.GetPosition();
        originalPos = new Vector2(pos.x - style.left.value.value, pos.y - style.top.value.value);
    }

    public void Reset()
    {
        style.left = 0;
        style.top = 0;
        //line = null;
    }
    private void OnDragStart(PointerDownEvent evt)
    {
        isDragging = true;
        Select();
        AddToClassList("selected");
    }

    private void OnDragEnd(PointerUpEvent evt)
    {
        onDragEnd?.Invoke(this);
        isDragging = false;
        RemoveFromClassList("selected");
    }

    private void OnDrag(PointerMoveEvent evt)
    {
        if (isDragging)
        {
            SetDraggedPos(evt.position);
        }
    }

    public void SetDraggedPos(Vector2 pos)
    {
        float elementWidth = resolvedStyle.width;
        float elementHeight = resolvedStyle.height;

        var clampedPos = new Vector2(
            Math.Clamp(pos.x, bodyEl.worldBound.x + elementWidth / 2, bodyEl.worldBound.x + bodyEl.worldBound.width - elementWidth / 2),
            Math.Clamp(pos.y, bodyEl.worldBound.y + elementHeight / 2, bodyEl.worldBound.y + bodyEl.worldBound.height - elementHeight / 2)
        );

        var adjustedPos = new Vector2(
            clampedPos.x - originalPos.x - elementWidth / 2,
            clampedPos.y - originalPos.y - elementHeight / 2
        );

        style.left = adjustedPos.x;
        style.top = adjustedPos.y;
    }
}