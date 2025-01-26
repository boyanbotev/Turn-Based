using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [SerializeField] bool isEnemy = false;

    private void OnEnable()
    {
        if (isEnemy) GameManager.onEnemyHealthChanged += UpdateHealth;
        else GameManager.onHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        if (isEnemy) GameManager.onEnemyHealthChanged -= UpdateHealth;
        else GameManager.onHealthChanged -= UpdateHealth;
    }

    void UpdateHealth(int health)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < health; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }


        foreach (Transform child in transform)
        {
            StartCoroutine(FlashRoutine(child, 2));
        }
    }

    IEnumerator FlashRoutine(Transform child, int flashCount)
    {
        var initialColor = child.GetComponent<Image>().color;

        float elapsedTime = 0f;
        float flashTime = 0.3f;

        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            child.GetComponent<Image>().color = Color.Lerp(Color.white, initialColor, (elapsedTime / flashTime));

            yield return null;
        }

        flashCount--;
        if (flashCount > 0)
        {
            StartCoroutine(FlashRoutine(child, flashCount));
        }
    }
}
