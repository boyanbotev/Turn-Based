using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAnim : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        GameManager.onWordCompleted += OnWordCompleted;
    }

    private void OnDisable()
    {
        GameManager.onWordCompleted -= OnWordCompleted;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnWordCompleted(string word)
    {
        StartCoroutine(HideAndShow());
    }
    public void Show()
    {
        animator.SetTrigger("show");
    }

    public void Hide()
    {
        animator.SetTrigger("hide");
    }

    IEnumerator HideAndShow()
    {
        //Hide();
        yield return new WaitForSeconds(3f);
        //Show();
    }
}
