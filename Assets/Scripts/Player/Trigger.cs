using NUnit;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ThrowableObject : MonoBehaviour
{
    public GameObject ball;
    public float throwForce ;//player
    public Material ballCol;
    public bool throwToTarget = false; //Aight gonn cook here lol this bool is always going to sit at false ma duddeeeeeeeeee 
  
    public Transform target ;
    public Transform[] enemyTarget;
    private Rigidbody rb;
    public bool enemyCanHit;
    public float hitDistance;//enemy
     public float throwHeight;//enemy
    public float launchAngle; //ignore   // ps dont ignore
    private score scoreBoard;

    private void Start()
    {
         rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -14f, 0);
        scoreBoard = GameObject.FindGameObjectWithTag("Ball").GetComponent<score>();
        rb.isKinematic = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowTrigger"))
        {

            throwToTarget = true;
            ballCol.SetColor ("_BaseColor", Color.red);

            if (throwToTarget && Input.GetKeyDown(KeyCode.Mouse0) == true)
            {
                ThrowObjectToTarget(target.position);
                Debug.Log("Target in Aight");
            }
            else
            {
               // ThrowObject();
            }
        }
        if (other .CompareTag("Enemy"))
        {
            enemyCanHit = true;
        }

        if (other.CompareTag("PlayerWall"))
        {
            OpponentScored();
        }

        if (other.CompareTag("OpponentWall"))
        {
            PlayerScored();
        }
    }

    private void Update()
    {
        int randomTargets = Random.Range (0, enemyTarget.Length);
        Vector3 finalTarget = enemyTarget[randomTargets].position;

        if (throwToTarget && Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            ThrowObjectToTarget(target.position);
            Debug.Log("Target in Aight");
        }
        if (enemyCanHit == true)
        {
            EnemyHit(finalTarget);
        }
    }

    private void OnTriggerExit()
    {
        throwToTarget = false; 
        enemyCanHit = false;
        ballCol.SetColor("_BaseColor",Color.green);
    }

    private void OnCollisionEnter(Collision collision)
    {
       //make a bounce meethod
    }

    public void ThrowObject()
    {


        Rigidbody rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = false;

        Vector3 throwDirection = transform.forward;
        Vector3 throwVelocity = throwDirection * throwForce;
        throwVelocity.y = throwHeight;

        rb.linearVelocity = throwVelocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void ThrowObjectToTarget(Vector3 targetPosition)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().isTrigger = false;
        Vector3 idkTarget = (rb.position - targetPosition).normalized;
        Vector3 trueTarget = transform.position + idkTarget; 
        Vector3 velocity = CalculateLaunchVelocity(trueTarget, targetPosition, launchAngle);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float angle)
    {
        float gravity = Physics.gravity.magnitude;
        float horizontalDistance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
        float effectiveDistance = Mathf.Min(horizontalDistance, throwForce);
        float velocity = effectiveDistance/ (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
         velocity = Mathf.Clamp(velocity,5f, 30f); //Come back here for ball launch speeds if shit gets slow 
        Debug.Log($"Raw Distance: {horizontalDistance} | Capped Distance: {effectiveDistance}");




        Vector3 direction = end - start;
        direction.y = 0;
        direction.Normalize();

        Vector3 launchVelocity = direction * velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        launchVelocity.y = velocity * Mathf.Sin(angle * Mathf.Deg2Rad);

        launchVelocity = launchVelocity.normalized * Mathf.Clamp(velocity, 5f, 30f);
        Debug.Log($"Calculated Velocity: {velocity} | From Distance: {horizontalDistance} | Angle: {angle}?");
        return launchVelocity;
    }
    public Vector3 EnemyHitVelocity(Vector3 startE, Vector3 endE,float angleE)
    {
        float gravity = Physics.gravity.magnitude;
        float horizontalDistance = Vector3.Distance(new Vector3(startE.x, 0, startE.z), new Vector3(endE.x, 0, endE.z));
        float effectiveDistance = Mathf.Min(horizontalDistance, hitDistance);
        float velocity = effectiveDistance / (Mathf.Sin(2 * angleE * Mathf.Deg2Rad) / gravity);
        velocity = Mathf.Clamp(velocity, 5f, 30f); //Come back here for ball launch speeds if shit gets slow 


        Vector3 direction = endE - startE;
        direction.y = 0;
        direction.Normalize();

        Vector3 launchVelocity = direction * velocity * Mathf.Cos(angleE * Mathf.Deg2Rad);
        launchVelocity.y = velocity * Mathf.Sin(angleE * Mathf.Deg2Rad);

        launchVelocity = launchVelocity.normalized * Mathf.Clamp(velocity, 5f, 30f);
        return launchVelocity;
    }
    public void EnemyHit(Vector3 enemyTarget)
    {
        Vector3 velocity = EnemyHitVelocity(transform.position, enemyTarget,throwHeight);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }


    void PlayerScored()
    {
        ball.SetActive(false);
        scoreBoard.AddScoreP();
        //Spawn eeefects and shi

        ball.transform.position = new Vector3 (6,1,-21);
        rb.isKinematic =true;
        throwToTarget = false;
        ball.SetActive (true);
        
        
    }

    void OpponentScored()
    {
        ball.SetActive(false);
        scoreBoard.AddScoreO();

        ball.transform.position = new Vector3(6, 1, -21);
        rb.isKinematic = true;
        throwToTarget = false;
        ball.SetActive(true);

    }
}
