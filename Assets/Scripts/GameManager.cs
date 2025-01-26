using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
    public static event Action<int> onHealthChanged;
    public static event Action<int> onEnemyHealthChanged;
    [SerializeField] Transform letterParent;
    [SerializeField] Transform writingLineParent;
    [SerializeField] private List<DraggableLetter> letters;
    [SerializeField] DragAndDropData[] dragAndDropDatas;
    [SerializeField] float writingLineXOffset = 2.5f;
    private int dragAndDropIndex = 0;
    private int health = 3; // TODO: move into different class
    private int enemyHealth = 5; // TODO: move into different class
    private float globalLetterZIndex = 0;

    [SerializeField] private GameObject writingLinePrefab;
    [SerializeField] private GameObject letterPrefab;


    private void OnEnable()
    {
        DraggableLetter.onSnapped += CheckIfWordComplete;
        DraggableObject.onSelected += UpdateLetterZIndex;
    }

    private void OnDisable()
    {
        DraggableLetter.onSnapped -= CheckIfWordComplete;
        DraggableObject.onSelected -= UpdateLetterZIndex;
    }

    private void Awake()
    {
        BuildChallenge();
    }

    void BuildChallenge()
    {
        BuildLetters();
        BuildWritingLines();
        BuildImage();
    }

    /// <summary>
    /// set image to the image of the word
    /// </summary>
    private void BuildImage()
    {
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

        for (int i = 0; i < dragAndDropDatas[dragAndDropIndex].word.Length; i++)
        {
            GameObject writingLine = Instantiate(writingLinePrefab, writingLineParent);

            // Add a gap between each writing line so the middle one is at 0
            var offset = i * writingLineXOffset - (dragAndDropDatas[dragAndDropIndex].word.Length - 1) * writingLineXOffset / 2;
            writingLine.transform.localPosition = new Vector3(offset, 0, 0);
        }
    }

    /// <summary>
    /// Add letters as appropriate in appropriate positions
    /// </summary>
    private void BuildLetters()
    {
        letters.Clear();
        foreach (Transform child in letterParent)
        {
            Destroy(child.gameObject);
        }

        // Add letters for ammount of letters in word
        string scrambledWord = ScrambleWord(dragAndDropDatas[dragAndDropIndex].word);

        for (int i = 0; i < scrambledWord.Length; i++)
        {
            GameObject letter = Instantiate(letterPrefab, letterParent);
            letter.transform.localPosition = new Vector3(i * writingLineXOffset - (scrambledWord.Length - 1) * writingLineXOffset / 2, 0, 0);
            letter.GetComponentInChildren<TextMeshPro>().text = scrambledWord[i].ToString();
            DraggableLetter draggableLetter = letter.GetComponent<DraggableLetter>();
            letters.Add(draggableLetter);
            draggableLetter.targets = writingLineParent;
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

        List<DraggableLetter> orderedLetters = GetOrderedLetters();

        for (int i = 0; i < orderedLetters.Count; i++)
        {
            word += orderedLetters[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        if (word == currentWord)
        {
            enemyHealth--;
            onEnemyHealthChanged?.Invoke(enemyHealth);
            StartCoroutine(CompleteWordRoutine(word));
            return;
        }

        health--;
        Debug.Log("wrong word: " + word + " not equal to " + currentWord);

        if (health == 0)
        {
            RestartScene();
            return;
        }

        RefreshChallenge();
    }
    private void RefreshChallenge()
    {
        onHealthChanged?.Invoke(health);
        BuildChallenge();
    }
    private void GoToNextChallenge()
    {
        if (dragAndDropIndex == dragAndDropDatas.Length - 1 || enemyHealth == 0)
        {
            GoToNextScene();
            return;
        }

        dragAndDropIndex++;
        globalLetterZIndex = 0;
        BuildChallenge();
    }

    private void AnimateWordToCentre()
    {
        // TODO: animate
        foreach (Transform child in writingLineParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in letterParent)
        {
            var pos = new Vector3(child.localPosition.x * 0.75f, child.localPosition.y, child.localPosition.z);
            child.localPosition = pos;
        }
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
    List<DraggableLetter> GetOrderedLetters()
    {
        List<DraggableLetter> orderedLetters = new List<DraggableLetter>();
        for (int i = 0; i < letters.Count; i++)
        {
            orderedLetters.Add(letters[i]);
        }
        orderedLetters.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        return orderedLetters;
    }

    void UpdateLetterZIndex(DraggableObject obj)
    {
        globalLetterZIndex -= 0.1f;
        obj.UpdateZIndex(globalLetterZIndex);
    }

    private IEnumerator CompleteWordRoutine(string word)
    {
        onWordCompleted?.Invoke(word);
        Debug.Log("word completed: " + word);
        AnimateWordToCentre();

        yield return new WaitForSeconds(1);

        GoToNextChallenge();
    }
}
