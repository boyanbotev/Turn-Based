using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DragAndDropData
{
    public string word;
    public Texture2D image;
}

public class GameManager : MonoBehaviour
{
    public static event Action<string> onWordCompleted;
    [SerializeField] Transform letterParent;
    [SerializeField] Transform writingLineParent;
    [SerializeField] private List<DraggableObject> letters;
    [SerializeField] DragAndDropData[] dragAndDropDatas;
    private int dragAndDropIndex = 0;

    [SerializeField] private GameObject writingLinePrefab;
    [SerializeField] private GameObject letterPrefab;


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
        BuildChallenge();
    }

    void BuildChallenge()
    {
        // Add letters as appropriate in appropriate positions
        BuildLetters();

        BuildWritingLines();


        // set image to the image of the word
        var image = GameObject.Find("image");
        var data = dragAndDropDatas[dragAndDropIndex];
        var sprite = Sprite.Create(data.image, new Rect(0, 0, data.image.width, data.image.height), new Vector2(0.5f, 0.5f));
        image.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private void BuildWritingLines()
    {
        // Remove writing lines
        // Add writing lines for ammount of letters in word
        foreach (Transform child in writingLineParent)
        {
            Destroy(child.gameObject);
        }
        float xOffset = 2.5f;

        for (int i = 0; i < dragAndDropDatas[dragAndDropIndex].word.Length; i++)
        {
            GameObject writingLine = Instantiate(writingLinePrefab, writingLineParent);

            // Add a gap between each writing line so the middle one is at 0
            var offset = i * xOffset - (dragAndDropDatas[dragAndDropIndex].word.Length - 1) * xOffset / 2;
            writingLine.transform.localPosition = new Vector3(offset, 0, 0);
        }
    }

    private void BuildLetters()
    {
        // Remove letters
        letters.Clear();

        foreach (Transform child in letterParent)
        {
            Destroy(child.gameObject);
        }
        // Add letters for ammount of letters in word

        float xOffset = 2.5f;

        string scrambledWord = ScrambleWord(dragAndDropDatas[dragAndDropIndex].word);

        for (int i = 0; i < scrambledWord.Length; i++)
        {
            GameObject letter = Instantiate(letterPrefab, letterParent);
            letter.transform.localPosition = new Vector3(i * xOffset - (scrambledWord.Length - 1) * xOffset / 2, 0, 0);
            letter.GetComponentInChildren<TextMeshPro>().text = scrambledWord[i].ToString();
            DraggableObject draggableObject = letter.GetComponent<DraggableObject>();
            letters.Add(draggableObject);
            draggableObject.targets = writingLineParent;
        }
    }

    string ScrambleWord(string word)
    {
        return new string(word.ToCharArray().OrderBy(x => Guid.NewGuid()).ToArray());
    }

    void CheckIfWordComplete()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            if (!letters[i].isSnapped)
            {
                return;
            }
        }

        string word = "";
        string currentWord = dragAndDropDatas[dragAndDropIndex].word;

        List<DraggableObject> orderedLetters = GetOrderedLetters();

        for (int i = 0; i < orderedLetters.Count; i++)
        {
            word += orderedLetters[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        if (word == currentWord)
        {
            GoToNextChallenge(word);
            return;
        }

        Debug.Log("wrong word: " + word + " not equal to " + currentWord);
        RefreshChallenge();
    }
    private void RefreshChallenge()
    {
        BuildChallenge();
    }
    private void GoToNextChallenge(string word)
    {
        onWordCompleted?.Invoke(word);
        Debug.Log("word completed: " + word);

        if (dragAndDropIndex == dragAndDropDatas.Length - 1)
        {
            GoToNextScene();
            return;
        }

        dragAndDropIndex++;
        BuildChallenge();
    }

    private static void GoToNextScene()
    {
        Debug.Log("All words completed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Order letters by x position from left to right
    List<DraggableObject> GetOrderedLetters()
    {
        List<DraggableObject> orderedLetters = new List<DraggableObject>();
        for (int i = 0; i < letters.Count; i++)
        {
            orderedLetters.Add(letters[i]);
        }
        orderedLetters.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        return orderedLetters;
    }

}
