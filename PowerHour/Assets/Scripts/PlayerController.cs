using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    [SerializeField] private PlayerInput  playerInput;

    InputAction lookAction;
    InputAction moveAction;

    public enum MoveState
    {
        Walking,
        Dashing
    };

    [Header("Cursor Parameters")]
    public  float mouseSensitivityHorizontal;
    public  float mouseSensitivityVertical;

    [Header("Walk Paramters")]
    public  float walkSpeed; // m/s
    public  float dashSpeed; // m/s

    [Header("Physics Parameters")]
    public  float playerHeight;
    public  float playerRadius;
    private float skinnyRadius;

    public  float skinWidth; // tolerance to prevent bounding box from intersecting with walls

    public  float velocityDecay; // m/s^2
    private float decayFactor;

    private MoveState moveState = MoveState.Walking;
    public  Vector3   acceleration; // m/s^2
    public  Vector3   velocity;     // m/s
    public  float     velocityMagnitude;
    public  float     accelerationMagnitude;

    void Awake()
    {
        lookAction   = playerInput.actions.FindAction("Look");
        moveAction   = playerInput.actions.FindAction("Move");

        skinnyRadius = playerRadius - skinWidth;

        decayFactor = 1 - velocityDecay * Time.fixedDeltaTime;
    }
    void Start()
    {
        Cursor.visible   = true; // make false and replace with custom cursor
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
            case MoveState.Walking:   return new(walkSpeed,   0, walkSpeed);
            case MoveState.Dashing:   return new(dashSpeed,   0, dashSpeed);
            default: return Vector3.zero;
        }
    }

    private void UpdateLook()
    {
        // shift camera based on cursor position on screen?
    }

    private void UpdatePosition()
    {
        (velocity, acceleration) = GetMovementComponents();

        velocityMagnitude     = velocity.magnitude;
        accelerationMagnitude = acceleration.magnitude;

        Vector3 actualDisplacement = CollideAndSlide(transform.position, velocity * Time.fixedDeltaTime);

        _ = CollideAndSlide(transform.position, velocity); // debug draw

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
        if(bounceCount >= 5)
        {
            return Vector3.zero; // prevent infinite recursion - return zero vector
        }

        // Shoot capsule cast and see if displacement will cause collision - return displacement if no collision
        if(Physics.CapsuleCast(origin + Vector3.up * playerRadius, origin + Vector3.up * (playerHeight - playerRadius), skinnyRadius, displacement.normalized, out RaycastHit hit, displacement.magnitude + skinWidth) == false)
        {
            Debug.DrawLine(origin, origin + displacement, Color.blue, Time.fixedDeltaTime);
            return displacement;
        }

        // Find new origin point (where the collision occurred)
        Vector3 reducedDisplacement  = displacement.normalized * (hit.distance - skinWidth);

        // Calculate leftover displacement after collision
        Vector3 leftoverDisplacement = displacement - reducedDisplacement;

        // Ensure there is enough room for collision check to work, otherwise set reducedDisplacement to zero
        if(reducedDisplacement.magnitude <= skinWidth)
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
}
