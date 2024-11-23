using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianController : MonoBehaviour
{
    MagicianAnim magicianAnim;

    void Start()
    {
        magicianAnim = GetComponent<MagicianAnim>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            magicianAnim.Burst();
        }
    }
}
