using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    public bool isRunning = false;
    public bool isJumping = false;
    public bool isClimbing = false;
    private bool hasJump;
    private bool hasClimb;
    public float climbSpeed = 0.2f;
    private bool hasClimbSpeed;

    void Start()
    {
        hasJump = HasParameter("isJumping");
        hasClimb = HasParameter("isClimbing");
        hasClimbSpeed = HasParameter("climbSpeed");
    }

    void Update()
    {
        if (animator.GetBool("isRunning") != isRunning) animator.SetBool("isRunning", isRunning);
        if (hasJump) if (animator.GetBool("isJumping") != isJumping) animator.SetBool("isJumping", isJumping);
        if (hasClimb) if (animator.GetBool("isClimbing") != isClimbing) animator.SetBool("isClimbing", isClimbing);
        if (hasClimbSpeed) if (animator.GetFloat("climbSpeed") != climbSpeed) animator.SetFloat("climbSpeed", climbSpeed);
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
