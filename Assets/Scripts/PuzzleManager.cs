using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

[System.Serializable]
public class JigsawData
{
    public string word;
    public Texture2D image;
}
public class PuzzleManager : MonoBehaviour
{
    public static event Action<string> onWordCompleted;
    [SerializeField] float puzzlePieceSpawnXBound = 7;
    [SerializeField] float puzzlePieceSpawnY = -3;
    [SerializeField] JigsawData[] jigsawData;
    [SerializeField] private DraggableObject[] puzzlePieces;
    private int currentPuzzleIndex = 0;

    private void OnEnable()
    {
        DraggableObject.onPuzzlePieceSnapped += CheckIfPuzzleIsComplete;
    }

    private void OnDisable()
    {
        DraggableObject.onPuzzlePieceSnapped -= CheckIfPuzzleIsComplete;
    }

    private void Start()
    {
        BuildPuzzle();
    }

    void BuildPuzzle()
    {
        // put puzzle pieces in random positions
        Vector2[] positions = new Vector2[puzzlePieces.Length];
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            Vector2 randomPosition = GetRandomPosition();
            while (IsPositionTooClose(randomPosition, positions))
            {
                randomPosition = GetRandomPosition();
            }

            positions[i] = randomPosition;
            puzzlePieces[i].transform.position = randomPosition;

            puzzlePieces[i].isSnapped = false;
        }

        // set tmpro text to letter of the word
        for (int i = 0; i < jigsawData[currentPuzzleIndex].word.Length; i++)
        {
            var text = puzzlePieces[i].gameObject.GetComponentInChildren<TextMeshPro>();
            text.text = jigsawData[currentPuzzleIndex].word[i].ToString();
        }

        // set image to the image of the word
        var image = GameObject.Find("Image");
        var sprite = Sprite.Create(jigsawData[currentPuzzleIndex].image, new Rect(0, 0, jigsawData[currentPuzzleIndex].image.width, jigsawData[currentPuzzleIndex].image.height), new Vector2(0.5f, 0.5f));
        image.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private bool IsPositionTooClose(Vector2 pos, Vector2[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            if (positions[i] != null)
            {
                if (Vector2.Distance(pos, positions[i]) < 3)
                {
                    return true;
                }
            }
        }
        return false;
    }

    Vector2 GetRandomPosition()
    {
        return new Vector2(UnityEngine.Random.Range(-puzzlePieceSpawnXBound, puzzlePieceSpawnXBound), puzzlePieceSpawnY);
    }

    void CheckIfPuzzleIsComplete()
    {
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            if (!puzzlePieces[i].isSnapped)
            {
                return;
            }
        }

        EmitWord();
        StartCoroutine(NextChallengeRoutine());
    }

    void EmitWord()
    {
        string word = "";
        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            word += puzzlePieces[i].gameObject.GetComponentInChildren<TextMeshPro>().text;
        }
        onWordCompleted?.Invoke(word);
    }

    void LoadNextChallenge()
    {
        if (currentPuzzleIndex < jigsawData.Length - 1)
        {
            currentPuzzleIndex++;
            BuildPuzzle();
        }
        else
        {
            GoToNextScene();
        }
    }
    public void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator NextChallengeRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        LoadNextChallenge();
    }
}
