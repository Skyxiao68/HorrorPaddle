using NUnit;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ThrowableObject : MonoBehaviour
{
    public GameObject ball;
    public float throwForce;//player
    public Material ballCol;
    public bool throwToTarget = false; //Aight gonn cook here lol this bool is always going to sit at false ma duddeeeeeeeeee 
    public ParticleSystem playerScorePar;
    public ParticleSystem enemyScorePar;
    public Transform target;
    public Transform[] enemyTarget;
    private Rigidbody rb;
    public bool enemyCanHit;
    public float hitDistance;//enemy
    public float throwHeight;//enemy
    public float launchAngle; //ignore   // ps dont ignore
    private ScoreManager scoreBoard;
    private stevePlayer steveAi;
    public bool onMySide;
    public float minVelocity = 0.5f;

    [Header("Sounds")]
    public AudioClip hitSound;
    private AudioSource hitAud;
    public AudioClip bouceSound;
    private AudioSource bouceAud;

    [Header("Aggressive")]
    public float hitForceAggressive;
    public float aggressiveArc;
    public Transform[] aggroTargets;
    [Header("Defensive")]
    public float hitForceDefensive;
    public float defensiveArc;
    public Transform[] defTargets;
    public PlayerInputController inputControl;
    public float inputFire;

    private void Awake()
    {
        inputControl = new PlayerInputController();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -14f, 0);
        scoreBoard = GameObject.FindGameObjectWithTag("Ball").GetComponent<ScoreManager>();
        rb.isKinematic = true;
        steveAi = FindFirstObjectByType<stevePlayer>();
        hitAud = GetComponent<AudioSource>();
        bouceAud = GetComponent<AudioSource>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowTrigger"))
        {

            throwToTarget = true;
            ballCol.SetColor("_BaseColor", Color.red);

            if (throwToTarget && inputFire == 1)
            {

                ThrowObjectToTarget(target.position);
                Debug.Log("Target in Aight");
            }
            else
            {
                // ThrowObject();
            }
        }
        if (other.CompareTag("Enemy"))
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

    void FixedUpdate()
    {
        inputFire = inputControl.Gameplay.Fire.ReadValue<float>();

        if (throwToTarget && inputFire == 1)
        {
            

            ThrowObjectToTarget(target.position);
            hitAud.PlayOneShot(hitSound);
            Debug.Log("Target in Aight");
        }
        if (enemyCanHit == true)
        {
            StanceDetector();
        }
        // if (rb.linearVelocity.z < 0.5)
        //{
        // onMySide = true;
        // }
        // else
        // {
        //    onMySide= false;
        //}  Im working on it 
    }

    private void OnTriggerExit()
    {
        throwToTarget = false;
        enemyCanHit = false;
        ballCol.SetColor("_BaseColor", Color.green);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //make a bounce meethod
        PlayBouceSound(collision.relativeVelocity.magnitude);

    }

    void PlayBouceSound(float impactVelocity)
    {

        if (impactVelocity > minVelocity && bouceSound != null)
        {
            bouceAud.PlayOneShot(bouceSound);
        }


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



    public void StanceDetector()
    {
        if (steveAi == null) return;
        Debug.Log("The ball stances are working");

        if (steveAi.stance == 1)
        {
            int randomTargetA = Random.Range(0, aggroTargets.Length);
            Aggressive(aggroTargets [randomTargetA].position );

            Debug.Log("Ball is Aggressive ");
        }
        if (steveAi.stance == 2)    
        {
            int randomTargetD = Random.Range(0,defTargets.Length);
            Defensive(defTargets[randomTargetD].position);
            Debug.Log("Ball is Defensive");
        }
    }
    public void EnemyHit(Vector3 enemyTarget) //yeah no sphagetti g=cooooding GGGGEZ Its not even been 2 weeks aahhhhhhhhhhhhhbhhhh scared to remove this lol
    {
        Vector3 velocity = EnemyHitVelocity(transform.position, enemyTarget,throwHeight);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void Aggressive(Vector3 aggroTarget)
    {
        hitDistance = hitForceAggressive;
        Vector3 velocity = EnemyHitVelocity(transform.position, aggroTarget, aggressiveArc);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    public void Defensive(Vector3 defTarget)
    {
        hitDistance = hitForceDefensive;
        Vector3 velocity = EnemyHitVelocity(transform.position, defTarget, defensiveArc);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }


    void PlayerScored()
    {

        Instantiate(playerScorePar,transform.position,transform.rotation);

        ball.SetActive(false);
        scoreBoard.AddScoreP();
     

        ball.transform.position = new Vector3 (10,1,-21);
        rb.isKinematic =true;
        throwToTarget = false;
        ball.SetActive (true);
        
        
    }

    void OpponentScored()
    {
        Instantiate(enemyScorePar, transform.position, transform.rotation);
        ballCol.SetColor("_BaseColor", Color.green);
        ball.SetActive(false);
        scoreBoard.AddScoreO();

        ball.transform.position = new Vector3(9, 1, -21);
        rb.isKinematic = true;
        throwToTarget = false;
        ball.SetActive(true);

    }
}
