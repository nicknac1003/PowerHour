using UnityEngine;

// This is a class that represents a basic melee enemy in the game
public class MeleeTank : Enemy
{
    public override void Init(){
        base.Init();
        id = "MeleeTank";
        maxHealth = 400;
        currentHealth = 400;
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
                Debug.Log("MeleeTank is attacking the player");
            }
        }
    }
}