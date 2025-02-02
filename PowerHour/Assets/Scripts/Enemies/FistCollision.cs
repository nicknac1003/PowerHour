using UnityEngine;

public class FistCollision : MonoBehaviour
{
    private MeleeGrunt enemyScript;
    private int damage;

    private void Start()
    {
        enemyScript = transform.GetComponentInParent<MeleeGrunt>();
        if (enemyScript != null)
        {
            //Debug.Log("Found component in parent: " + component.gameObject.name);
            damage = enemyScript.damage;
        }
        else
        {
            Debug.Log("Component not found in any parent.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player Hitbox")
        {
            other.gameObject.GetComponentInParent<PlayerAnimationController>().TakeDamage(damage);
        }
    }
}
