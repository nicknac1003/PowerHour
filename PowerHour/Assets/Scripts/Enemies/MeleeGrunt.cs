using UnityEngine;

// This is a class that represents a basic melee enemy in the game
public class MeleeGrunt : Enemy
{
    public float _currhp;
    public float _maxhp;

    [SerializeField]
    public Transform attackPoint;

    public int damage = 25;

    [Header("Player Dammage Colliders")]
    [SerializeField] private Collider rightHandCollider;
    [SerializeField] private Collider leftHandCollider;
    public override void Init()
    {
        base.Init();
        id = "MeleeGrunt";
        maxHealth = _maxhp;
        currentHealth = _currhp;
        target = GameObject.Find("Player");
        rightHandCollider.enabled = false;
        leftHandCollider.enabled = false;

        DisableLHandCollider();
        DisableRHandCollider();
    }
    public override void attack()
    {
        // This is where the enemy would attack the player
        // For now, we will just print a message
        if (target != null)
        {
            //if the player is within range, attack the player
            float distanceToPlayer = Vector3.Distance(this.transform.position, target.transform.position);
            if (distanceToPlayer <= range)
            {
                int attackChoice = Random.Range(0, 2);
                switch (attackChoice)
                {
                    case 0:
                        animator.SetTrigger("punch");
                        break;
                    case 1:
                        animator.SetTrigger("uppercut");
                        break;
                }
                Debug.Log("MeleeGrunt is attacking the player");
            }
        }
    }

    public void EnableRHandCollider()
    {
        rightHandCollider.enabled = true;
    }
    public void DisableRHandCollider()
    {
        rightHandCollider.enabled = false;
    }
    public void EnableLHandCollider()
    {
        leftHandCollider.enabled = true;
    }
    public void DisableLHandCollider()
    {
        leftHandCollider.enabled = false;
    }
    public void RPunchEnd()
    {
        attacking = false;
    }
    public void UppercutEnd()
    {
        attacking = false;
    }
}