using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    [SerializeField] private int kickDmg = 10;
    [SerializeField] private int punchDmg = 5;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy Hitbox")
        {
            other.gameObject.GetComponentInParent<Enemy>().TakeDamage(punchDmg);
        }
    }
}
