using UnityEngine;
using System.Collections;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private FPSController playerController;
    [SerializeField] private float punchAnimSpeed = 1.0f;
    [SerializeField] private float punchCooldown = 1.0f;
    [SerializeField] private float turnSpeed = 1000.0f;
    private float nextPunchTime = 0.0f;

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
        StartPunch();

    }

    private void StartPunch()
    {

        // on left click, punch
        if (Input.GetMouseButtonDown(0) && Time.time >= nextPunchTime)
        {
            Debug.Log("Punch");
            animator.SetTrigger("Punch");
            playerController.currentlyPunching = true;
            animator.SetFloat("AnimSpeed", punchAnimSpeed); //here for debugging, remove later
            // Look at mouse location when punching
            Quaternion targetRotation = playerController.GetMouseLocation();
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }
    }

    // For when all punch animations are done, and we are idle/running
    // Currently bugged. You can start RPunch animation before cd applies if you hold down movement keys while spamming punch
    void PunchEnd()
    {
        Debug.Log("PunchEnd");
        nextPunchTime = Time.time + punchCooldown; //reset punch cd
        playerController.currentlyPunching = false;
    }

    // Functions called by animations:
    void RPunchEnd()
    {
        animator.SetTrigger("RPunchOver");
    }

    void LPunchEnd()
    {
        animator.SetTrigger("LPunchOver");
    }

    void KickEnd()
    {
        animator.SetTrigger("KickOver");
    }
}
