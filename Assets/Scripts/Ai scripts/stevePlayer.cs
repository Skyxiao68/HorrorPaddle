// Wrote this mostly by myself yayyyy
//Used last semester's scripts for pointers and this video for pointers as well on the 25 July 2025
// https://youtu.be/jvtFUfJ6CP8?si=v7R0Yft9adPUVVS 
using UnityEngine;
using UnityEngine.AI;

public class stevePlayer : MonoBehaviour
{
    private CharacterController steve;
    public Rigidbody ball;

    public int stance;
    public float stanceTimer;
    private float lastSwitch = Mathf.Infinity;
    public Material aiMaterial;
    public bool stanceSwitch;
    private ThrowableObject theBallStuck;

    [Header("Aggresive")]
    public float moveSpeedA;
    public float apredictionZ;
    public float apredictionX;

    [Header("Defensive")]
    public float moveSpeedD;
    public float dpredictionX;
    public float dpredictionZ;

    [Header("Clamp Settings")]
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    private Vector3 movementVelocity;

    private void Awake()
    {
        steve = GetComponent<CharacterController>();
        theBallStuck = FindFirstObjectByType<ThrowableObject>();

        // Make sure NavMeshAgent is disabled if not used
        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null) navAgent.enabled = false;

        stance = 1;
        stanceSwitch = false;
        lastSwitch = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && stanceSwitch)
        {
            stance = (stance == 1) ? 2 : 1;
            lastSwitch = Time.time;
            stanceSwitch = false;
        }
    }

    void Update()
    {
        if (stance == 1)
        {
            AggressiveMovement();
        }
        else if (stance == 2)
        {
            DefensiveMovement();
        }

        if (!stanceSwitch && Time.time > lastSwitch + stanceTimer)
        {
            stanceSwitch = true;
            Debug.Log($"StanceSwitch: {stanceSwitch}");
        }

        ApplyMovement();
    }

    void AggressiveMovement()
    {
        aiMaterial.SetColor("_BaseColor", Color.red);

        float predictionTimeZ = apredictionZ;
        float ballVelocityZ = ball.linearVelocity.z;
        float predictedBallZ = ball.position.z + (ballVelocityZ * predictionTimeZ);

        Vector3 moveDirection = new Vector3(
            ball.position.x - transform.position.x,
            0,
            predictedBallZ - transform.position.z
        ).normalized;

        movementVelocity = moveDirection * moveSpeedA;

        // DEBUG: Add this to see what's happening with Z tracking
        Debug.Log($"Aggressive - Ball Z: {ball.position.z}, Predicted Z: {predictedBallZ}, My Z: {transform.position.z}, Velocity Z: {ballVelocityZ}");
    }

    void DefensiveMovement()
    {
        aiMaterial.SetColor("_BaseColor", Color.blue);

        float ballVelocityX = ball.linearVelocity.x;
        float predictionTimeX = dpredictionX;
        float predictedBallX = ball.position.x + (ballVelocityX * predictionTimeX);

        float ballVelocityZ = ball.linearVelocity.z;
        float predictionTimeZ = dpredictionZ;
        float predictedBallZ = ball.position.z + (ballVelocityZ * predictionTimeZ);

        Vector3 moveDirection = new Vector3(
            predictedBallX - transform.position.x,
            0,
            predictedBallZ - transform.position.z
        ).normalized;

        movementVelocity = moveDirection * moveSpeedD;

        // DEBUG: Add this to see what's happening with Z tracking
        Debug.Log($"Defensive - Ball Z: {ball.position.z}, Predicted Z: {predictedBallZ}, My Z: {transform.position.z}, Velocity Z: {ballVelocityZ}");
    }

    void ApplyMovement()
    {
        // Calculate target position with clamping
        Vector3 targetMovement = movementVelocity * Time.deltaTime;

        // Apply movement using CharacterController (proper way)
        steve.Move(targetMovement);

        // Clamp position after CharacterController movement
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 currentPosition = transform.position;
        Vector3 clampedPosition = currentPosition;

        clampedPosition.x = Mathf.Clamp(currentPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(currentPosition.z, minZ, maxZ);

        // Only update position if it changed significantly
        if (Vector3.Distance(currentPosition, clampedPosition) > 0.01f)
        {
            // Proper way to reposition CharacterController
            steve.enabled = false;
            transform.position = clampedPosition;
            steve.enabled = true;
        }
    }

    // Visualize the clamp area in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((minX + maxX) / 2, transform.position.y, (minZ + maxZ) / 2);
        Vector3 size = new Vector3(maxX - minX, 1f, maxZ - minZ);
        Gizmos.DrawWireCube(center, size);
    }
}