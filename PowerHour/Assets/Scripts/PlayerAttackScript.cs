using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private int kickDmg = 10;
    [SerializeField] private int punchDmg = 5;
    private void OnCollisionEnter(Collision other)
    {

        // Janky, but prevents me from having to add a oncollisionenter script to each player attack collider
        foreach (ContactPoint contact in other.contacts)
        {
            // Check if the collider on THIS object that participated in the collision has the required tag.
            if (contact.thisCollider.CompareTag("PlayerHurtCollider"))
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Player hit enemy");
                    other.gameObject.GetComponent<Enemy>().TakeDamage(punchDmg);
                }
                break;
            }
        }
    }
}
