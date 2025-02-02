using UnityEngine;

// This is a class that represents a basic melee enemy in the game
public class RangedGrunt : Enemy
{
    public float _currhp;
    public float _maxhp;


    public GameObject holdParent;
    public GameObject projectile;
    public GameObject holding;
    public override void Init(){
        base.Init();
        id = "RangedGrunt";
        maxHealth = _maxhp;
        currentHealth = _currhp;
    }

    public float reloadTime = 2f;

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
                animator.SetTrigger("Throw");
                Debug.Log("RangedGrunt is attacking the player");
            }
        }
    }

    public override void Update(){
        base.Update();
        if (lastAttackTime + reloadTime < Time.time && holding == null){
            // Debug.Log("Reloading");
            reload();
        }
    }

    public void reload(){
        holding = Instantiate(projectile, holdParent.transform.position, holdParent.transform.rotation);
        holding.transform.parent = holdParent.transform;
        // holding.GetComponent<bottle>().holdPoint = holdParent.transform;
        // holding.GetComponent<Rigidbody>().useGravity = false;
    }

    public override void move()
    {
        float distanceToPlayer = Vector3.Distance(this.transform.position, target.transform.position);
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0;
        if (distanceToPlayer > range)
        {
            Debug.Log(!attacking);
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
            // this.transform.Rotate(0, 60, 0); //rotate extra for fighting stance to line up

            if (Time.time > lastAttackTime + attackDelay && holding != null)
            {
                // Debug.Log("attacking");
                attacking = true;
                lastAttackTime = Time.time;
                attack();
            }
        }
    }

    public void release(){
        if (holding != null){

            holding.transform.parent = null;
            holding.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 targetThrow = target.transform.position;
            targetThrow.y += 1.25f;
            //draw point at targteThrow
            Debug.DrawLine(targetThrow, targetThrow + Vector3.up, Color.green, 5);

            //get vector toward target

            float angle = 45f;
            Vector3 velocity = calcVelocity(targetThrow, 10f);
            holding.GetComponent<Rigidbody>().linearVelocity = velocity;
            
            //apply random spin force
            holding.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere * 10, ForceMode.Impulse);
            holding = null;
        }
    }

    public Vector3 calcVelocity(Vector3 target, float speed)
    {  

        // Calculate direction to target but ignore the y component to only rotate on the X and Z axes
        Vector3 directionToTargetFlat = target - holding.transform.position;
        directionToTargetFlat.y = 0; // This zeroes out the vertical component

        // Create a rotation that looks at the target along the X and Z axes
        Quaternion lookRotation = Quaternion.LookRotation(directionToTargetFlat);

        // Apply the rotation to the object. 
        // If you want to preserve the object's original Y rotation, you could adjust this step accordingly.
        transform.rotation = lookRotation;

        Vector3 toTarget = target - holding.transform.position;
        // Visualize the direction to the target
        Debug.DrawRay(holding.transform.position, toTarget, Color.green, 2.0f);

        // Set up the terms we need to solve the quadratic equations.
        float gSquared = Physics.gravity.sqrMagnitude;
        float b = speed * speed + Vector3.Dot(toTarget, Physics.gravity);    
        float discriminant = b * b - gSquared * toTarget.sqrMagnitude;

        // Check whether the target is reachable at max speed or less.
        if(discriminant < 0) {
            // Target is too far away to hit at this speed.
            // Abort, or fire at max speed in its general direction?
            Debug.Log("Abort!");
        }

        float discRoot = Mathf.Sqrt(discriminant);

        // Highest shot with the given max speed:
        float T_max = Mathf.Sqrt((b + discRoot) * 2f / gSquared);

        // Most direct shot with the given max speed:
        float T_min = Mathf.Sqrt((b - discRoot) * 2f / gSquared);

        // Lowest-speed arc available:
        float T_lowEnergy = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f/gSquared));
        
        // Given max distance = 10, the closer the target, T is closer to T_min and vice versa
        float T = (toTarget.magnitude/10 * (T_max - T_min))/4 + T_min;

        // Convert from time-to-hit to a launch velocity:
        Vector3 velocity = toTarget / T - Physics.gravity * T / 2f;

        // Apply the calculated velocity (do not use force, acceleration, or impulse modes)
        return velocity;
    
    }
}