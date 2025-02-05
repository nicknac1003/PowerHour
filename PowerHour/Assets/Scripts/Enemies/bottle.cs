using UnityEngine;

class bottle : MonoBehaviour
{

    //particle
    public GameObject shatter_effect;
    private AudioSource audioSource;
    public AudioClip shatterSound;
    private MeshRenderer meshRenderer;
    private Collider collider;
    public bool isShattered = false;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
    }

    //on collision with player, destroy the bottle and deal damage to the player
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Bottle collided with " + other.gameObject.tag);
        // IDamageable other = collision.gameObject.GetComponent<IDamageable>();
        if (other.gameObject.tag == "Player Hitbox")
        {
            other.gameObject.GetComponentInParent<PlayerAnimationController>().TakeDamage(10);
            other.gameObject.GetComponentInParent<PlayerController>().TakeDamage(10);
        }
        shatter();

    }

    private void shatter()
    {
        isShattered = true;
        audioSource.clip = shatterSound;
        audioSource.Play();
        meshRenderer.enabled = false;
        collider.enabled = false;
        GameObject effect = Instantiate(shatter_effect, transform.position, transform.rotation);
        effect.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 3f);
    }
}