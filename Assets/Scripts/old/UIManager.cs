using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    string[] letters;
    [SerializeField]
    UIDocument uiDoc;
    VisualElement mainEl;
    VisualElement draggableLettersEl;

    private void Awake()
    {
        var root = uiDoc.rootVisualElement;
        mainEl = root.Q<VisualElement>(className: "main");
    }

    private void OnEnable()
    {
        DraggableLetter.onDragEnd += OnDragEnd;
    }

    private void OnDisable()
    {
        DraggableLetter.onDragEnd -= OnDragEnd;
    }

    private void Start()
    {
        CreateDraggableLetters();
    }

    private void CreateDraggableLetters()
    {
        draggableLettersEl = new VisualElement();
        draggableLettersEl.AddToClassList("draggable-letters");
        mainEl.Add(draggableLettersEl);

        foreach (var letter in letters)
        {
            var draggableLetter = new DraggableLetter(letter, mainEl);
            draggableLettersEl.Add(draggableLetter);
        }
    }

    void OnDragEnd(DraggableLetter dragged)
    {
        // Add them to the writing line text

        // Check if the word is correct

        // if correct, animate the letters together

        // if not correct, reset the letters
    }

    /**
     * What should this class be responsible for?
     * 
     * doesn't matter, it's a prototype class
     * 
     * 
     */
}
