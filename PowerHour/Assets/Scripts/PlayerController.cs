using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] public Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();

    public float maxHP=100;
    private float _currentHealth;
    private float _maxHealth;

    public float currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public float maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public bool isDead = false;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    [SerializeField] Transform crosshair;

    InputAction lookAction;
    InputAction moveAction;
    InputAction attackAction;

    public enum MoveState
    {
        Walking,
        Dashing
    };

    [Header("Camera Parameters")]
    public Vector3 cameraOffset;
    public float cameraSpeed;
    public float maxCrosshairDistance;
    public float verticalScale; // less vertical room, so scale up input for better visibility

    [Header("Cursor Parameters")]
    public float mouseSensitivityHorizontal;
    public float mouseSensitivityVertical;
    private Vector3 crosshairWorldPosition;
    private Vector3 dampingVelocity;

    [Header("Walk Paramters")]
    public float walkSpeed; // m/s
    public float dashSpeed; // m/s
    public float turnSpeed; // degrees/s

    [Header("Physics Parameters")]
    public float playerHeight;
    public float playerRadius;
    private float skinnyRadius;

    public float skinWidth; // tolerance to prevent bounding box from intersecting with walls

    public float velocityDecay; // m/s^2
    private float decayFactor;

    private MoveState moveState = MoveState.Walking;
    public Vector3 acceleration; // m/s^2
    public Vector3 velocity;     // m/s
    public float velocityMagnitude;
    public float accelerationMagnitude;
    public bool currentlyPunching = false;

    [Header("HealthBar")]
    public GameObject healthBarUI;
    private Slider healthBar;
    private TextMeshProUGUI healthText;


    void Awake()
    {
        lookAction    = playerInput.actions.FindAction("Look");
        moveAction    = playerInput.actions.FindAction("Move");
        attackAction = playerInput.actions.FindAction("Attack");

        skinnyRadius = playerRadius - skinWidth;

        decayFactor = 1 - velocityDecay * Time.fixedDeltaTime;


    }
    void Start()
    {
        Cursor.visible = true; // make false and replace with custom cursor

        maxHealth = maxHP;
        currentHealth = maxHealth;


        healthBar = healthBarUI.GetComponentInChildren<Slider>();
        healthText = healthBarUI.GetComponentsInChildren<TextMeshProUGUI>()[1];
        Debug.Log(healthBar.value + " " + healthText.text);
        healthBar.value = 1;
        
    }
    void Update()
    {
        UpdateLook();
        UpdateBuffs();
        UpdateAttack();
        UpdateHealthBar();
    }

    void FixedUpdate()
    {
        UpdatePosition();
    }

    private Vector3 GetMoveSpeed()
    {
        switch (moveState)
        {
            case MoveState.Walking: return new(walkSpeed, 0, walkSpeed);
            case MoveState.Dashing: return new(dashSpeed, 0, dashSpeed);
            default: return Vector3.zero;
        }
    }

    //TODO: Slow turn speed a little as you get more drunk?
    private void UpdateLook()
    {
        // Update crosshair location
        UpdateCrosshairLocation();

        UpdateCameraLocation();
        

        if (!currentlyPunching)
        {
            // point y rotation towards velocity
            if (velocity != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                transform.rotation = targetRotation;
            }
        }

    }

    private void UpdateCameraLocation()
    {
        // Move camera to follow player
        Vector3 toCrosshair = ToCrosshair();
        float crosshairDistance = Mathf.Min(toCrosshair.magnitude, maxCrosshairDistance);
        Vector3 crosshairTarget = toCrosshair.normalized * crosshairDistance;
        
        // Scale crosshairTarget along 45 degree line
        Vector3 projectedVertical   = Vector3.Project(crosshairTarget, new Vector3(1, 0, 1));   // forward is +X & +Z
        Vector3 projectedHorizontal = Vector3.Project(crosshairTarget, new Vector3(1, 0 , -1)); // right   is +X & -Z

        projectedVertical *= verticalScale;

        crosshairTarget = projectedVertical + projectedHorizontal;

        Vector3 targetPosition = transform.position + cameraOffset + crosshairTarget;
        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref dampingVelocity, 1f / cameraSpeed);
    }

    private void UpdateCrosshairLocation()
    {
        crosshairWorldPosition = Vector3.zero;

        if(crosshair == null)
        {
            Debug.LogWarning("Crosshair not assigned!");
            return;
        }

        if(Physics.Raycast(Camera.main.ScreenPointToRay(crosshair.position), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
        {
            crosshairWorldPosition = new Vector3(hit.point.x, 0, hit.point.z); // ALWAYS on floor level (Y = 0)
            return;
        }

        Debug.LogWarning("Crosshair did not hit collider!");
    }

    // Get the transform of the mouse location in the world (on the floor) for attacks.
    public Quaternion GetAngleToTagret()
    {
        Vector3 directionToHitPoint = ToCrosshair();
        return Quaternion.Euler(0, Mathf.Atan2(directionToHitPoint.x, directionToHitPoint.z) * Mathf.Rad2Deg, 0);;
    }
    public Vector3 ToCrosshair()
    {
        return crosshairWorldPosition - transform.position;
    }

    private void UpdatePosition()
    {
        (velocity, acceleration) = CalculateMovement();

        velocityMagnitude = velocity.magnitude;
        accelerationMagnitude = acceleration.magnitude;

        Vector3 actualDisplacement = CollideAndSlide(transform.position, velocity * Time.fixedDeltaTime);
        actualDisplacement.y = 0;

        _ = CollideAndSlide(transform.position, velocity); // debug draw

        if (currentlyPunching)
        {
            actualDisplacement *= 0.1f;
        }

        transform.position += actualDisplacement;

        transform.position = new Vector3(transform.position.x, 0, transform.transform.position.z); // ALWAYS on floor level (Y = 0)
    }

    private (Vector3 v, Vector3 a) CalculateMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>().normalized; // wish direction

        Vector3 moveSpeed = GetMoveSpeed(); // max move speeds for this state

        Vector3 magnitude = moveSpeed * velocityDecay / decayFactor;

        Vector3 horizontalAcceleration = GlobalConstants.forward * input.y * magnitude.x + GlobalConstants.right * input.x * magnitude.z; // m/s^2

        Vector3 horizontalVelocity = (new Vector3(velocity.x, 0f, velocity.z) + horizontalAcceleration * Time.fixedDeltaTime) * decayFactor; // m/s

        return (horizontalVelocity, horizontalAcceleration);
    }

    private Vector3 CollideAndSlide(Vector3 origin, Vector3 displacement, int bounceCount = 0)
    {
        if (bounceCount >= 5)
        {
            return Vector3.zero; // prevent infinite recursion - return zero vector
        }

        // Shoot capsule cast and see if displacement will cause collision - return displacement if no collision
        if (Physics.CapsuleCast(origin + Vector3.up * playerRadius, origin + Vector3.up * (playerHeight - playerRadius), skinnyRadius, displacement.normalized, out RaycastHit hit, displacement.magnitude + skinWidth, LayerMask.GetMask("Wall")) == false)
        {
            Debug.DrawLine(origin, origin + displacement, Color.blue, Time.fixedDeltaTime);
            return displacement;
        }

        // Find new origin point (where the collision occurred)
        Vector3 reducedDisplacement = displacement.normalized * (hit.distance - skinWidth);

        // Calculate leftover displacement after collision
        Vector3 leftoverDisplacement = displacement - reducedDisplacement;

        // Ensure there is enough room for collision check to work, otherwise set reducedDisplacement to zero
        if (reducedDisplacement.magnitude <= skinWidth)
        {
            reducedDisplacement = Vector3.zero;
        }

        Vector3 projectedDisplacement = Vector3.ProjectOnPlane(leftoverDisplacement, hit.normal);

        Vector3 newOrigin = origin + displacement.normalized * hit.distance;

        Debug.DrawLine(origin + reducedDisplacement, origin + displacement, Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(origin, origin + reducedDisplacement, Color.green, Time.fixedDeltaTime);
        Debug.DrawLine(origin, hit.point, Color.yellow, Time.fixedDeltaTime);

        return reducedDisplacement + CollideAndSlide(newOrigin, projectedDisplacement, bounceCount + 1);
    }


    private void UpdateAttack()
    {
        if(attackAction.triggered)
        {
            playerAnimationController.StartPunch();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("Player has died");
    }

    private void UpdateBuffs()
    {
        foreach (var buff in _buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                _buffs.Remove(buff.Buff);
            }
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        if (_buffs.ContainsKey(buff.Buff))
        {
            _buffs[buff.Buff].Activate();
        }
        else
        {
            _buffs.Add(buff.Buff, buff);
            buff.Activate();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public float CalculateHealth()
    {
        // Debug.Log("Health: " + currentHealth / maxHealth);
        return currentHealth / maxHealth;
    }

    public void UpdateHealthBar()
    {
        Debug.Log("Updating health bar " + healthBar.value + " " + healthText.text);
        healthBar.value = CalculateHealth();
        healthText.text = Mathf.Max(currentHealth, 0) + "/" + maxHealth;
    }
}
