using NUnit.Framework;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    public bool isRunning = false;

    void Update()
    {
        if (animator.GetBool("isRunning") != isRunning) animator.SetBool("isRunning", isRunning);
    }
    
}
