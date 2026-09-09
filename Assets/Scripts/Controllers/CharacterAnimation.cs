using NUnit.Framework;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    public bool isRunning = false;
    public bool isJumping = false;
    private bool hasJump;

    void Start()
    {
        hasJump = HasParameter("isJumping");
    }

    void Update()
    {
        if (animator.GetBool("isRunning") != isRunning) animator.SetBool("isRunning", isRunning);
        if (hasJump) if (animator.GetBool("isJumping") != isJumping) animator.SetBool("isJumping", isJumping);
    }

    private bool HasParameter(string param)
    {
        foreach (AnimatorControllerParameter par in animator.parameters)
        {
            if (par.name == param) return true;
        }
        return false;
    }
    
}
