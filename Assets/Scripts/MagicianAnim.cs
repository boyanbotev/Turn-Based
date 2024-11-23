using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianAnim : MonoBehaviour
{
    [SerializeField]
    GameObject orb;
    [SerializeField]
    GameObject ray;
    Animator orbAnim;
    Animator rayAnim;

    void Start()
    {
        orbAnim = orb.GetComponent<Animator>();
        rayAnim = ray.GetComponent<Animator>();
    }

    public void Burst()
    {
        orbAnim.SetTrigger("burst");
        rayAnim.SetTrigger("burst");
    }

}
