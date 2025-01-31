using UnityEngine;
using UnityEngine.UI;
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
    private bool attacking = false;

    public bool isHit = false;

    public GameObject healthBarUI;
    public Slider healthBar;
    
    private Camera mainCamera;

    protected Animator animator;
    public virtual void attack()
    {
        return;
    }
    public virtual void move()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, target.transform.position);
        if (distanceToPlayer > range)
        {
            if (!isHit && !attacking)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("inCombat", false);
                //look at player without looking up or down
                Vector3 direction = target.transform.position - this.transform.position;
                direction.y = 0;
                this.transform.rotation = Quaternion.LookRotation(direction);

                this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + transform.forward, speed* Time.deltaTime);
            } 
            else if (Time.time > lastHitTime + hitDelay)
            {
                isHit = false;
            }
        } else
        {
            animator.SetBool("isWalking", false);
            bool enterCombat = !animator.GetBool("inCombat");
            animator.SetBool("inCombat", true);
            //rotate 60 degrees on y axis
            if (enterCombat)
            {
                this.transform.Rotate(0, 60, 0);
            }
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
        Init();
    }

    public virtual void Init()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator != null);
        if (healthBarUI != null)
        {
            healthBar.value = CalculateHealth();
        }
        return;
    }
    public virtual void Update()
    {
        if (healthBarUI != null && healthBar != null)
        {
            healthBar.value = CalculateHealth();
            if (currentHealth < maxHealth)
            {
                healthBarUI.SetActive(true);
            } else
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
        return currentHealth / maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time > lastHitTime + hitDelay)
        {
            lastHitTime = Time.time;
            currentHealth -= damage;
            isHit = true;
            if (currentHealth <= 0)
            {
                Destroy(gameObject, 0.5f);
            }
        }

    }

    public void AttackDone()
    {
        Debug.Log("Attack done");
        attacking = false;
    }

}
