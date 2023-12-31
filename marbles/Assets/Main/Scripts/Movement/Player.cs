using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    Transform visualRepresentation = default;

    [SerializeField, Range(0f, 100f)]
    float maxForwardSpeed = 20f, midForwardSpeed = 15f, lowForwardSpeed = 5f, maxHorizontalSpeed = 5f, maxSnapSpeed = 5f;

    [SerializeField, Range(0f, 100f)]
    float maxForwardAcceleration = 10f, maxBackAcceleration = 1f, maxBackAccelerationInSlow = 10f, maxHorizontalAcceleration = 10f;

    [SerializeField, Range(0f, 90f)]
    float maxGroundAngle = 25f, maxStairsAngle = 50f;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    LayerMask probeMask = -1, stairsMask = -1;

    [SerializeField]
    Material normalMaterial = default;

    [SerializeField, Min(0.1f)]
    float ballRadius = 0.5f;

    [SerializeField]
    float radius = 256f;

    [SerializeField]
    float turnTax = 25f;

    Vector3 velocity;
    float playerInput;
    Rigidbody body;

    int groundContactCount, steepContactCount;

    bool OnGround => groundContactCount > 0;

    bool OnSteep => steepContactCount > 0;

    float minGroundDotProduct, minStairsDotProduct;

    Vector3 contactNormal, steepNormal;
    int stepsSinceLastGrounded;

    Vector3 upAxis;

    MeshRenderer meshRenderer;
    
    public List<bool> SpeedSlowZones { get; set; } = new List<bool>();

    float forwardSpeedLastTick = 0;

    bool allowStartRacing = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        meshRenderer = visualRepresentation.GetComponent<MeshRenderer>();
        OnValidate();
    }

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
    }

    public void Update()
    {
        playerInput = Mathf.Clamp(Input.GetAxis("Horizontal"), -1, 1);

        UpdateBall();
    }

    private void FixedUpdate()
    {
        if (!allowStartRacing)
        {
            return;
        }

        // find out speedstate
        float forwardSpeedFull = GetForwardSpeedFull();

        Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);

        // Lookforward depends on the curvature of the track
        Vector3 closestPoint = ClosestPointOnCircle(body.position, radius);
        Vector3 outerDirection = closestPoint.normalized;
        Vector3 forwardDirection = Quaternion.AngleAxis(90, Vector3.up) * outerDirection;

        // Rotate ball, so that it always aligns with the track
        transform.rotation = Quaternion.LookRotation(forwardDirection, upAxis);

        UpdateState();
        AdjustVelocity(forwardSpeedFull);

        if (OnGround && velocity.sqrMagnitude < 0.01f)
        {
            velocity += contactNormal * (Vector3.Dot(gravity.normalized, contactNormal) * Time.deltaTime);
        }
        else
        {
            velocity += gravity * Time.deltaTime;
        }
        

        body.velocity = velocity;
        ClearState();
    }

    public float GetForwardSpeedFull()
    {
        if (SpeedSlowZones.Count == 0)
        {
            return midForwardSpeed;
        }
        else if (SpeedSlowZones.Contains(true))
        {
            return maxForwardSpeed;
        }
        else
        {
            return lowForwardSpeed;
        }
    }

    public Vector3 ClosestPointOnCircle(Vector3 position, float radius)
    {
        // Circles origin is assumed to be the center
        return new Vector3(position.x, 0, position.z).normalized * radius;
    }

    void UpdateBall()
    {
        Material ballMaterial = normalMaterial;

        meshRenderer.material = ballMaterial;

        float velocityMagPerFrame = Vector3.Dot(body.velocity, body.transform.forward) * Time.deltaTime;

        float angle = velocityMagPerFrame * (180f / Mathf.PI) / ballRadius;
        visualRepresentation.localRotation = Quaternion.Euler(Vector3.right * angle) * visualRepresentation.localRotation;
    }

    void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero;
    }

    void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        velocity = body.velocity;

        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
            
        }
        else
        {
            contactNormal = upAxis;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        int layer = collision.gameObject.layer;
        float minDot = GetMinDot(layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float upDot = Vector3.Dot(upAxis, normal);
            if (upDot >= minDot)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
            else
            {
                if (upDot > -0.01f)
                {
                    steepContactCount += 1;
                    steepNormal += normal;
                }
            }
        }
    }

    bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            float upDot = Vector3.Dot(upAxis, steepNormal);
            if (upDot >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }

        return false;
    }

    void AdjustVelocity(float forwardSpeedFull)
    {
        Vector3 xAxis, zAxis;

        xAxis = ProjectDirectionOnPlane(transform.right, contactNormal);
        zAxis = ProjectDirectionOnPlane(transform.forward, contactNormal);

        float horizontalSpeed;
        horizontalSpeed = playerInput * maxHorizontalSpeed - Vector3.Dot(velocity, xAxis);
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxHorizontalAcceleration * Time.deltaTime, maxHorizontalAcceleration * Time.deltaTime);

        float forwardSpeed;
        // Velocity throttles itself like this without ever needing to clamp it.

        // Turn tax
        //Debug.Log(horizontalSpeed);
        //float forwardSpeedMinusTax = forwardSpeedFull - Mathf.Abs(playerInput * maxHorizontalSpeed);

        forwardSpeedFull -= Mathf.Abs(playerInput * maxHorizontalSpeed * turnTax * Time.deltaTime);
        forwardSpeedFull = Mathf.Max(forwardSpeedFull, lowForwardSpeed);

        forwardSpeed = forwardSpeedFull - Vector3.Dot(velocity, zAxis);

        if (forwardSpeed - forwardSpeedLastTick >= 0)
        {
            forwardSpeed = Mathf.Clamp(forwardSpeed, -maxForwardAcceleration * Time.deltaTime, maxForwardAcceleration * Time.deltaTime);
        }
        else
        {
            float backAcceleration;

            if (SpeedSlowZones.Contains(false))
            {
                backAcceleration = maxBackAccelerationInSlow;
            }
            else
            {
                backAcceleration = maxBackAcceleration;
            }

            forwardSpeed = Mathf.Clamp(forwardSpeed, -backAcceleration * Time.deltaTime, backAcceleration * Time.deltaTime);
        }        

        forwardSpeedLastTick = forwardSpeed;

        velocity += xAxis * horizontalSpeed + zAxis * forwardSpeed;
    }

    Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
    {
        return (direction - normal * Vector3.Dot(direction, normal)).normalized;
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast(body.position, -upAxis, out RaycastHit hit, probeDistance, probeMask))
        {
            return false;
        }
        float upDot = Vector3.Dot(upAxis, hit.normal);
        if (upDot < GetMinDot(hit.collider.gameObject.layer))
        {
            return false;
        }

        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }

        return true;
    }

    float GetMinDot(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ? minGroundDotProduct : minStairsDotProduct;
    }

    public void AllowStartRacing(bool startRacing)
    {
        allowStartRacing = startRacing;
    }
}
