using UnityEngine;

// This is a class that represents a basic melee enemy in the game
public class RangedGrunt : Enemy
{
    public float _currhp;
    public float _maxhp;

    public GameObject projectile;
    public GameObject holding;
    public override void Init(){
        base.Init();
        id = "RangedGrunt";
        maxHealth = _maxhp;
        currentHealth = _currhp;
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
                animator.SetTrigger("Throw");
                Debug.Log("RangedGrunt is attacking the player");
            }
        }
    }
}