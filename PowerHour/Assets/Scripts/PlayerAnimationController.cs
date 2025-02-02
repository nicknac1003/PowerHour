using UnityEngine;
using System.Collections;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float punchAnimSpeed = 1.0f;
    [SerializeField] private float punchCooldown = 1.0f;
    [SerializeField] private float turnSpeed = 1000.0f;
    private float timeUntilNextPunch = 0.0f;

    int VelocityHash;
    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");
        animator.SetFloat("AnimSpeed", punchAnimSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(VelocityHash, playerController.velocityMagnitude);
        //One could argue that the Punching function shouldn't be in the animation script but oopsies! We only have 3 days to finish!
        if (timeUntilNextPunch > 0)
        {
            timeUntilNextPunch -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("CanStartCombo", true);
        }

        StartPunch();
    }



    private void StartPunch()
    {
        // on left click, punch
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("PunchQueued");
            animator.SetFloat("AnimSpeed", punchAnimSpeed); //here for debugging, remove later
            /* // Look at mouse location when punching
            Quaternion targetRotation = playerController.GetMouseLocation();
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );*/
        }
    }

    public void PunchOver()
    {
        playerController.currentlyPunching = false;
        animator.ResetTrigger("PunchQueued");
    }

    // Functions called by animations:
    // Functions are in order of combo: rpunch, lpunch, then kick
    void RPunchBegin()
    {
        playerController.currentlyPunching = true;
    }
    void RPunchEnd()
    {
        animator.SetTrigger("RPunchOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
    }

    void LPunchEnd()
    {
        animator.SetTrigger("LPunchOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
    }

    void KickEnd()
    {
        animator.SetTrigger("KickOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
    }
}
