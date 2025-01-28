using UnityEngine;

// This is a class that represents a basic melee enemy in the game
public class MeleeElite : Enemy
{
    public override void Init(){
        base.Init();
        id = "MeleeElite";
        maxHealth = 200;
        currentHealth = 200;
    }
    public override void attack()
    {
        // This is where the enemy would attack the player
        // For now, we will just print a message
        if (target != null && model != null)
        {
            //if the player is within range, attack the player
            float distanceToPlayer = Vector3.Distance(model.transform.position, target.transform.position);
            if (distanceToPlayer <= range)
            {
                Debug.Log("MeleeElite is attacking the player");
            }
        }
    }
}