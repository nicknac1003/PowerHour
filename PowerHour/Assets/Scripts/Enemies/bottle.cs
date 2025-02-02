using UnityEngine;

class bottle : MonoBehaviour {

    //particle
    public GameObject shatter_effect;
    public void Start(){
    }

    public void Update(){
        //if y within .1 of 0, destroy the bottle
        if (transform.position.y < 0.1){
            shatter();
        }
    }

    //on collision with player, destroy the bottle and deal damage to the player
    public void OnTriggerEnter(Collider other){
        Debug.Log("Bottle collided with " + other.gameObject.tag);
        // IDamageable other = collision.gameObject.GetComponent<IDamageable>();
        if (other.gameObject.tag == "Player Hitbox")
        {
            other.gameObject.GetComponentInParent<PlayerAnimationController>().TakeDamage(10);
            other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(10);
            shatter();
        }

    }

    public void shatter(){
        GameObject effect = Instantiate(shatter_effect, transform.position, transform.rotation);
        effect.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}