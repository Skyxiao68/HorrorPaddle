




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
    public Transform greenTarget;
    public Transform blueTarget;
    public Transform magentaTarget;
    public Transform[] enemyTarget;
    private Rigidbody rb;

    public float hitDistance;//enemy
    public float throwHeight;//enemy
    public float launchAngle; //ignore   // ps dont ignore
    private ScoreManager scoreBoard;
    private stevePlayer steveAi;
    private sarahPlayer sarahAi;
    private batDirection batStance;
    private batDirection.batState currentBatState;
    public Transform currentTarget;
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

    [Header ("ZaWorldo Ball")]
    //public float DioTimeScale = 0.2f;
    public float hitSpeedMulti = 1.2f; 
    private bool isInDioTime = false;
    private Vector3 originalVelocity;
    private Vector3 originalAngularVelocity;
    private bool ballWashit = false;

    public bool iWantScore;
    private void Awake()
    {
        inputControl = new PlayerInputController();
    }

    private void OnEnable()
    {
        inputControl.Enable();
        
        EventManage.AddListener("DioTimeStarted", StartDioTime);
        EventManage.AddListener("DioTimeEnded", EndDioTime);

        TimeManage.Instance.OnDioTimeScaleChanged += OnDioTimeScaleChanged;
    }

    private void OnDisable()
    {
        inputControl.Disable();

        EventManage.RemoveListener("DioTimeStarted", StartDioTime);
        EventManage.RemoveListener("DioTimeEnded", EndDioTime);

        if (TimeManage.Instance != null)
        {
            TimeManage.Instance.OnDioTimeScaleChanged -= OnDioTimeScaleChanged;
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -14f, 0);
        scoreBoard = GameObject.FindGameObjectWithTag("Ball").GetComponent<ScoreManager>();
       
        rb.isKinematic = true;
        steveAi = FindFirstObjectByType<stevePlayer>();
        sarahAi = FindFirstObjectByType<sarahPlayer>();
        hitAud = GetComponent<AudioSource>();
        bouceAud = GetComponent<AudioSource>();
        batStance = FindFirstObjectByType<batDirection>();

    }
    private void OnDioTimeScaleChanged(float newScale)
    {
        if (isInDioTime)
        {
            
            rb.linearVelocity = originalVelocity * newScale;
            rb.angularVelocity = originalAngularVelocity * newScale;
        }
    }

    private void StartDioTime()
    {
        if (isInDioTime ) return;

        isInDioTime = true;
        ballWashit = false;
      

        originalVelocity = rb.linearVelocity;
        originalAngularVelocity = rb.angularVelocity;

        rb.linearVelocity *= TimeManage.Instance.dioTimeScale;
        rb.angularVelocity *= TimeManage.Instance.dioTimeScale;


        Debug.Log("Ball entered Zaworldo");

    
    }

    private void EndDioTime()
    {
        if (!isInDioTime) return; 

        isInDioTime = false;
        

        


        if (!ballWashit) 
        {
            rb.linearVelocity = originalVelocity;
            rb.angularVelocity = originalAngularVelocity;
        }

      
        

        Debug.Log("Ball exited Zaworldo "); 
    }

 

    public void OnBallHit()
    {
        if (isInDioTime)
        {
            ballWashit = true;

            rb.linearVelocity = originalVelocity * hitSpeedMulti;
            rb.angularVelocity = originalAngularVelocity;



            Debug.Log("Ball was hit,it got STANDO POWER!! ");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowTrigger"))
        {

            throwToTarget = true;
            ballCol.SetColor("_BaseColor", Color.red);

            if (throwToTarget && inputFire == 1)
            {

                ThrowObjectToTarget(currentTarget.position);
                Debug.Log("Target in Aight");
            }
            else
            {
                // ThrowObject();
            }
        }
        if (other.CompareTag("Enemy"))
        {
            // Instead of just setting a bool, find out which AI script is on the enemy that hit us.
            stevePlayer hittingSteve = other.GetComponent<stevePlayer>();
            sarahPlayer hittingSarah = other.GetComponent<sarahPlayer>();

            if (hittingSteve != null)
            {
                UseEnemyAi(hittingSteve.stance);
            }
            else if (hittingSarah != null)
            {
                UseEnemyAi(hittingSarah.jumpStance);
            }

            
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
        if (batStance != null)
        {
            currentBatState = batStance.currentState;
            PlayerBat();
        }

        inputFire = inputControl.Gameplay.Fire.ReadValue<float>();

        if (throwToTarget && inputFire == 1)
        {
            
            
            ThrowObjectToTarget(currentTarget.position);
            hitAud.PlayOneShot(hitSound);
            Debug.Log("Target in Aight");

           
        }

      
        
      
       
    }
    void PlayerBat()
    {
        switch (currentBatState)
        {
            case batDirection.batState.Forward:
                currentTarget = greenTarget; break;

            case batDirection.batState.Left:
                currentTarget = magentaTarget; break;

            case batDirection.batState.Right:
                currentTarget = blueTarget; break;
        }
    }

    private void OnTriggerExit()
    {
        throwToTarget = false;
     
        ballCol.SetColor("_BaseColor", Color.green);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //make a bounce meethod
        PlayBouceSound(collision.relativeVelocity.magnitude);

        if (collision.gameObject.CompareTag("Player") && isInDioTime)
        {
            OnBallHit(); 
        }



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
        OnBallHit();

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
        OnBallHit();

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



   public void UseEnemyAi(int enemyStance)
    {
        Debug.Log("An enemy hit the ball! Their stance is: " + enemyStance);

        if (enemyStance == 1) // Aggressive Stance
        {
            int randomTargetA = Random.Range(0, aggroTargets.Length);
            Aggressive(aggroTargets[randomTargetA].position);
            Debug.Log("Ball is Aggressive ");
        }
        else if (enemyStance == 2) // Defensive Stance
        {
            int randomTargetD = Random.Range(0, defTargets.Length);
            Defensive(defTargets[randomTargetD].position);
            Debug.Log("Ball is Defensive");
        }

        if (isInDioTime)
        {
            rb.linearVelocity = originalVelocity;
            rb.angularVelocity = originalAngularVelocity;
        }
    }


    
    public void EnemyHit(Vector3 enemyTarget) //yeah no sphagetti g=cooooding GGGGEZ Its not even been 2 weeks aahhhhhhhhhhhhhbhhhh scared to remove this lol
    {
        
        
        Vector3 velocity = EnemyHitVelocity(transform.position, enemyTarget,throwHeight);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;


        if (isInDioTime)
        {
            rb.linearVelocity = originalVelocity;
            rb.angularVelocity = originalAngularVelocity;
        }
    }

    public void Aggressive(Vector3 aggroTarget)
    {
    

        hitDistance = hitForceAggressive;
        Vector3 velocity = EnemyHitVelocity(transform.position, aggroTarget, aggressiveArc);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (isInDioTime)
        {
            rb.linearVelocity = originalVelocity;
            rb.angularVelocity = originalAngularVelocity;
        }
    }

    public void Defensive(Vector3 defTarget)
    {
        

        hitDistance = hitForceDefensive;
        Vector3 velocity = EnemyHitVelocity(transform.position, defTarget, defensiveArc);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;

        if (isInDioTime)
        {
            rb.linearVelocity = originalVelocity;
            rb.angularVelocity = originalAngularVelocity;
        }
    }


    void PlayerScored()
    {
        if (iWantScore ==false) { return; }
        Instantiate(playerScorePar,transform.position,transform.rotation);
        bouceAud.PlayOneShot(bouceSound);
        ball.SetActive(false);
        scoreBoard.AddScoreP();
     

        ball.transform.position = new Vector3 (7.49f, 1.12f, -80);
        rb.isKinematic =true;
        throwToTarget = false;
        ball.SetActive (true);

        
        
    }

    void OpponentScored()
    {
        if (iWantScore ==false) {return; }
        Instantiate(enemyScorePar, transform.position, transform.rotation);
        bouceAud.PlayOneShot(bouceSound);
        ballCol.SetColor("_BaseColor", Color.green);
        ball.SetActive(false);
        scoreBoard.AddScoreO();

        ball.transform.position = new Vector3(7.49f, 1.12f, -80);
        rb.isKinematic = true;
        throwToTarget = false;
        ball.SetActive(true);

        

    }
}
