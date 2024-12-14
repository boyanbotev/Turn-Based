using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
