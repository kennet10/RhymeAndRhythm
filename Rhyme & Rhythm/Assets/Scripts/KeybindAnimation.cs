using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeybindAnimation : MonoBehaviour
{
    public Animator animator;

    public void Open()
    {
        animator.SetBool("clickx", false);
    }

    public void Close()
    {
        animator.SetBool("clickx", true);
    }
}
