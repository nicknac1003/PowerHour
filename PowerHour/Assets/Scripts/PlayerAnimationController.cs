using UnityEngine;
using System.Collections;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private FPSController playerController;
    [SerializeField] private float RPunchAnimSpeed = 1.0f;
    int VelocityHash;
    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");

    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(VelocityHash, playerController.velocityMagnitude);

        // on left click, punch
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Punch");
            animator.speed = RPunchAnimSpeed;
            playerController.velocity = Vector3.zero;
            transform.rotation = playerController.GetMouseLocation();
        }
    }

    void RPunchEnd()
    {
        //animator.speed = 1.0f; // set speed back to normal for the animator
        animator.SetTrigger("RPunchOver");
    }

    void LPunchEnd()
    {
        animator.speed = 1.0f; // set speed back to normal for the animator
        animator.SetTrigger("LPunchOver");
    }

    void KickEnd()
    {
        animator.speed = 1.0f; // set speed back to normal for the animator
        animator.SetTrigger("KickOver");
    }
}
