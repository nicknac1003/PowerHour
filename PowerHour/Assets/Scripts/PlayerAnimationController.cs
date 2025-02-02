using UnityEngine;
using System.Collections;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    [Header("Animation Properties")]
    [SerializeField] private float punchAnimSpeed = 1.0f;
    [SerializeField] private float punchCooldown = 1.0f;
    [SerializeField] private float turnSpeed = 1000.0f;

    [Header("Player Dammage Colliders")]
    [SerializeField] private Collider rightHandCollider;
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider leftFootCollider;
    private float timeUntilNextPunch = 0.0f;

    int VelocityHash;
    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");
        animator.SetFloat("AnimSpeed", punchAnimSpeed);

        rightHandCollider.enabled = false;
        leftHandCollider.enabled = false;
        leftFootCollider.enabled = false;
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
                                                            // Look at mouse location when punching
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
        rightHandCollider.enabled = true;
        Quaternion targetRotation = playerController.GetMouseLocation();
        transform.rotation = targetRotation;
    }
    void RPunchEnd()
    {
        animator.SetTrigger("RPunchOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
        rightHandCollider.enabled = false;
    }

    void LPunchBegin()
    {
        leftHandCollider.enabled = true;
    }
    void LPunchEnd()
    {
        animator.SetTrigger("LPunchOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
        leftHandCollider.enabled = false;

    }

    void KickBegin()
    {
        leftFootCollider.enabled = true;
    }
    void KickEnd()
    {
        animator.SetTrigger("KickOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
        leftFootCollider.enabled = false;
    }
}
