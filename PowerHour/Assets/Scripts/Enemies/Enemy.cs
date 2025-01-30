using UnityEngine;

public interface IDamageable
{
    int maxHealth { get; set; }
    int currentHealth { get; set; }
    void TakeDamage(int damage);
}

// This is a class that represents an enemy in the game
public class Enemy : MonoBehaviour, IDamageable
{
    private int _currentHealth;
    private int _maxHealth;

    public int currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public int maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }


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

    public bool isHit = false;
    
    public virtual void attack()
    {
        return;
    }
    public virtual void move()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, target.transform.position);
        if (distanceToPlayer > range)
        {
            if (!isHit)
            {
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
            if (Time.time > lastAttackTime + attackDelay)
            {
                lastAttackTime = Time.time;
                attack();
            }
        }
    }


    public void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        return;
    }
    public virtual void Update()
    {
        move();
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

    

}
