using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    public enum HitType
    {
        Punch,
        Kick
    }
    public HitType hitType = HitType.Punch;
    [SerializeField] private float damage = 10;
    private PlayerAnimationController playerAnimationController;

    private void Start()
    {
        playerAnimationController = this.GetComponentInParent<PlayerAnimationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // get tag of the object that is doign the hitting
        if (other.gameObject.tag == "Enemy Hitbox")
        {
            other.gameObject.GetComponentInParent<IDamageable>().TakeDamage(damage);

            if (hitType == HitType.Punch)
            {
                playerAnimationController.PlayPunchHitSound();
            }
            else
            {
                playerAnimationController.PlayKickHitSound();
            }
        }
        if (other.gameObject.tag == "jukebox")
        {
            other.gameObject.GetComponent<Jukebox>().RotateSong();
        }
    }

    public void increaseDamage(float amount)
    {
        damage += amount;
    }
}
