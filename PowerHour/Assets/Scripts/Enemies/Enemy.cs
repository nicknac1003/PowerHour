using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
// This is a class that represents an enemy in the game
public class Enemy : MonoBehaviour, IDamageable
{
    private float _currentHealth;
    private float _maxHealth;

    public float currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public float maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }


    [SerializeField]
    public float baseSpeed;

    [SerializeField]
    public float speed;

    [SerializeField]
    public float range;

    [SerializeField]
    public string id;

    [SerializeField]
    public GameObject target;

    protected float lastHitTime;
    [SerializeField]
    public float hitDelay;

    protected float lastAttackTime;

    [SerializeField]
    public float attackDelay;
    protected bool attacking = false;

    protected bool isHit = false;

    public GameObject healthBarUI;
    public Slider healthBar;

    private Camera mainCamera;

    protected Animator animator;

    int smallHitLaunchFactor = 10;
    int bigHitLaunchFactor = 2;
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    [SerializeField] private Collider hitbox;

    public virtual void attack()
    {
        return;
    }
    public virtual void move()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, target.transform.position);
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0;
        if (distanceToPlayer > range)
        {
            if (!isHit && !attacking)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("inCombat", false);
                //look at player without looking up or down

                this.transform.rotation = Quaternion.LookRotation(direction);

                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + transform.forward, speed * Time.deltaTime);
            }
            else if (Time.time > lastHitTime + hitDelay)
            {
                isHit = false;
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            bool enterCombat = !animator.GetBool("inCombat");
            animator.SetBool("inCombat", true);

            this.transform.rotation = Quaternion.LookRotation(direction);
            //this.transform.Rotate(0, 60, 0); //rotate extra for fighting stance to line up

            if (Time.time > lastAttackTime + attackDelay)
            {
                attacking = true;
                lastAttackTime = Time.time;
                attack();
            }
        }
    }


    public void Start()
    {
        mainCamera = Camera.main;
        if (healthBarUI != null)
        {
            healthBar.value = 1;
        }
        Init();
    }

    public virtual void Init()
    {
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
        rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        return;
    }
    public virtual void Update()
    {
        //if keypress q then take damage
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(10);
        }

        if (healthBarUI != null && healthBar != null)
        {
            healthBar.value = CalculateHealth();
            if (currentHealth < maxHealth)
            {
                healthBarUI.SetActive(true);
            }
            else
            {
                healthBarUI.SetActive(false);
            }
            healthBarUI.transform.LookAt(mainCamera.transform);
            healthBarUI.transform.Rotate(0, 180, 0);
        }

        move();

    }

    public float CalculateHealth()
    {
        // Debug.Log("Health: " + currentHealth / maxHealth);
        return currentHealth / maxHealth;
    }
    private void Die()
    {
        Destroy(animator);
        Destroy(healthBarUI);
        Destroy(hitbox);
        Destroy(this);
    }

    public void TakeDamage(float damage)
    {
        if (Time.time > lastHitTime + hitDelay)
        {
            lastHitTime = Time.time;
            currentHealth -= damage;
            isHit = true;

            if (currentHealth <= 0)
            {
                Die();
                //Destroy(gameObject, 6f);
            }
            else
            {
                animator.SetTrigger("Hit");
                Vector3 direction = this.transform.position - target.transform.position;
                direction.y = 0.0f;
            }
        }
    }

    private void ApplyForce(Vector3 direction, int launchFactor)
    {
        if (direction != Vector3.zero)
        {
            direction.Normalize();
        }
        Launch(direction, launchFactor);
    }

    public void Launch(Vector3 direction, int launchFactor)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddForce(direction * launchFactor, ForceMode.Impulse);
        }
    }

    public void AttackDone()
    {
        // Debug.Log("Attack done");
        attacking = false;
    }
}
