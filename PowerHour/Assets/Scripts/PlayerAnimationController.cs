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

    [Header("Audio Clips")]
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip punchHitSound;
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private AudioClip kickHitSound;

    private float timeUntilNextPunch = 0.0f;
    private AudioSource audioSource;

    int VelocityHash;
    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");
        animator.SetFloat("AnimSpeed", punchAnimSpeed);
        audioSource = GetComponent<AudioSource>();

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
    }

    public void StartPunch()
    {
        animator.SetTrigger("PunchQueued");
        animator.SetFloat("AnimSpeed", punchAnimSpeed); //here for debugging, remove later
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("PlayerHit");
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
        transform.rotation = playerController.GetAngleToTagret();
    }
    void EnableRHandCollider()
    {
        rightHandCollider.enabled = true;
    }
    void DisableRHandCollider()
    {
        rightHandCollider.enabled = false;
    }
    void RPunchEnd()
    {
        animator.SetTrigger("RPunchOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
    }

    void EnableLHandCollider()
    {
        leftHandCollider.enabled = true;
    }
    void DisableLHandCollider()
    {
        leftHandCollider.enabled = false;
    }

    void LPunchEnd()
    {
        animator.SetTrigger("LPunchOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
        leftHandCollider.enabled = false;

    }

    void EnableKickCollider()
    {
        leftFootCollider.enabled = true;
    }
    void DisableKickCollider()
    {
        leftFootCollider.enabled = false;
    }
    void KickEnd()
    {
        animator.SetTrigger("KickOver");
        animator.SetBool("CanStartCombo", false);
        timeUntilNextPunch = punchCooldown;
    }

    public void PlayPunchSound()
    {
        audioSource.clip = punchSound;
        audioSource.Play();
    }
    public void PlayPunchHitSound()
    {
        audioSource.clip = punchHitSound;
        audioSource.Play();
    }
    public void PlayKickSound()
    {
        audioSource.clip = kickSound;
        audioSource.Play();
    }
    public void PlayKickHitSound()
    {
        audioSource.clip = kickHitSound;
        audioSource.Play();
    }
}
