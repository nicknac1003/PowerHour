using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour, IDamageable
{
    public float maxHP=100;
    private float _currentHealth;
    private float _maxHealth;

    public float currentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
    public float maxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] Transform crosshairPos;

    InputAction lookAction;
    InputAction moveAction;

    public enum MoveState
    {
        Walking,
        Dashing
    };

    [Header("Cursor Parameters")]
    public float mouseSensitivityHorizontal;
    public float mouseSensitivityVertical;

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

    void Awake()
    {
        lookAction = playerInput.actions.FindAction("Look");
        moveAction = playerInput.actions.FindAction("Move");

        skinnyRadius = playerRadius - skinWidth;

        decayFactor = 1 - velocityDecay * Time.fixedDeltaTime;

        maxHealth = maxHP;
        currentHealth = maxHealth;
    }
    void Start()
    {
        Cursor.visible = true; // make false and replace with custom cursor
    }
    void Update()
    {
        UpdateLook();
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
        if (!currentlyPunching)
        {
            // 8 Directional movements based on wasd
            (velocity, acceleration) = GetMovementComponents();

            // point y rotation towards velocity
            if (velocity != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

                transform.rotation = targetRotation;
            }
        }

    }

    // Get the transform of the mouse location in the world (on the floor) for attacks.
    public Quaternion GetMouseLocation()
    {
        // Ensure the crosshair is assigned
        if (crosshairPos == null)
        {
            Debug.LogWarning("Crosshair not assigned!");
            return Quaternion.identity;
        }

        // Raycast from the camera through the crosshair to find the floor, then turn Ydir towards that point

        // Get the crosshair's position in screen space
        Vector3 crosshairScreenPosition = crosshairPos.position;

        // Create a ray from the camera through the crosshair position
        Ray ray = Camera.main.ScreenPointToRay(crosshairScreenPosition);

        // Perform the raycast to find where the crosshair hits the floor
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // Get the point where the raycast hits the floor
            Vector3 floorHitPoint = hit.point;

            // Calculate the direction from the character to the hit point
            Vector3 directionToHitPoint = floorHitPoint - transform.position;
            directionToHitPoint.y = 0; // Ignore vertical difference (Y-axis) for 2.5D rotation

            // Calculate the target rotation to face the hit point
            float targetAngle = Mathf.Atan2(directionToHitPoint.x, directionToHitPoint.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            return targetRotation;
        }

        Debug.Log("Crosshair not on screen, error on GetMouseLocation()");
        return Quaternion.identity;

    }

    private void UpdatePosition()
    {
        (velocity, acceleration) = GetMovementComponents();

        velocityMagnitude = velocity.magnitude;
        accelerationMagnitude = acceleration.magnitude;

        Vector3 actualDisplacement = CollideAndSlide(transform.position, velocity * Time.fixedDeltaTime);

        _ = CollideAndSlide(transform.position, velocity); // debug draw

        if (currentlyPunching)
        {
            actualDisplacement *= 0.1f;
        }

        transform.position += actualDisplacement;
    }

    private (Vector3 v, Vector3 a) GetMovementComponents()
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
        if (Physics.CapsuleCast(origin + Vector3.up * playerRadius, origin + Vector3.up * (playerHeight - playerRadius), skinnyRadius, displacement.normalized, out RaycastHit hit, displacement.magnitude + skinWidth) == false)
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("Player has died");
    }
}
