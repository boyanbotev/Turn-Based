using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianController : MonoBehaviour
{
    MagicianAnim magicianAnim;

    private void OnEnable()
    {
        GameManager.onWordCompleted += Burst;
    }

    private void OnDisable()
    {
        GameManager.onWordCompleted -= Burst;
    }

    void Start()
    {
        magicianAnim = GetComponent<MagicianAnim>();
    }

    void Burst(string word)
    {
        magicianAnim.Burst();
    }
}
