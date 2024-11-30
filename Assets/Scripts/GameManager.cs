using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    /**
     * TODO:
     * check when all letters are snapped
     * check if word is correct
     * 
     * if so play animation
     * 
     * 
     * 
     * 
     * 
     */

    public static event Action<string> onWordCompleted;
    [SerializeField] Transform letterParent;
    [SerializeField] private DraggableObject[] letters;
    private string currentWord = "sat";

    private void OnEnable()
    {
        DraggableObject.onPuzzlePieceSnapped += CheckIfWordComplete;
    }

    private void OnDisable()
    {
        DraggableObject.onPuzzlePieceSnapped -= CheckIfWordComplete;
    }

    private void Awake()
    {
        letters = letterParent.GetComponentsInChildren<DraggableObject>();
    }
    void CheckIfWordComplete()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            if (!letters[i].isSnapped)
            {
                return;
            }
        }

        string word = "";

        List<DraggableObject> orderedLetters = GetOrderedLetters();

        for (int i = 0; i < orderedLetters.Count; i++)
        {
            word += orderedLetters[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        if (word == currentWord)
        {
            onWordCompleted?.Invoke(word);
            Debug.Log("word completed: " + word);
            return;
        }

        Debug.Log("wrong word: " + word + " not equal to " + currentWord);
    }


    // Order letters by x position from left to right
    List<DraggableObject> GetOrderedLetters()
    {
        List<DraggableObject> orderedLetters = new List<DraggableObject>();
        for (int i = 0; i < letters.Length; i++)
        {
            orderedLetters.Add(letters[i]);
        }
        orderedLetters.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        return orderedLetters;
    }

}
