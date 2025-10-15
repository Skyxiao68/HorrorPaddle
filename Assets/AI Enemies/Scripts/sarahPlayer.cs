// Wrote this mostly by myself yayyyy
//Used last semester's scripts for pointers and this video for pointers as well on the 25 July 2025
// https://youtu.be/jvtFUfJ6CP8?si=v7R0Yft9adPUVVS 



using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class sarahPlayer : MonoBehaviour
{

    private CharacterController sarah;
    public Rigidbody ball;


    private bool offGround;
    public int jumpStance;
    public float stanceTimer;
    private float lastSwitch = Mathf.Infinity;
    private Vector3 ballHit;
    public Material aiMaterial;

    private bool canSwitchStance;
    private ThrowableObject theBallStuck;

    [Header("Aggresive")]
    public float moveSpeedA;
    public float apredictionZ;
    public float apredictionX;
    [Header("Defensive")]
    public float moveSpeedD;
    public float dpredictionX;
    public float dpredictionZ;

     public bool isJumping;
    public float jumpHeight;
    public float jumpCondition;
    public float jumpTime;

    [Header("Clamp Setting")] //HAhahahaha Im dumb time for NavMESH AGAIN Sat here for 2 hours building a box for this idiot 

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;

    [Header("ZaWorldo Debuff")]
    //public float dioTimeScale = 0.2f;
    private bool isInDioTime = false;
    private float originalMoveSpeedA;
    private float originalMoveSpeedD;
    private float originalJumpHeight;
    private float originalJumpTime;





    private void Awake()
    {
        sarah = GetComponent<CharacterController>();
       
        GetComponent<NavMeshAgent>().enabled = true;
        jumpStance = 1;
        canSwitchStance = false;
        lastSwitch = Time.time;

        originalMoveSpeedA = moveSpeedA;
        originalMoveSpeedD = moveSpeedD;
        originalJumpHeight = jumpHeight;
        originalJumpTime = jumpTime;

        EventManage.AddListener("DioTimeStarted", StartDioTime);
        EventManage.AddListener("DioTimeEnded", EndDioTime);

      //  TimeManage.Instance.OnDioTimeScaleChanged += OnDioTimeScaleChanged;
    }

    private void OnDestroy()
    {
        EventManage.RemoveListener("DioTimeStarted", StartDioTime);
        EventManage.RemoveListener("DioTimeEnded", EndDioTime);

        if (TimeManage.Instance != null)
        {
           // TimeManage.Instance.OnDioTimeScaleChanged -= OnDioTimeScaleChanged;
        }   

    }

    private void OnDioTimeScaleChanged(float newScale)
    {
        if (isInDioTime)
        {
            
            moveSpeedA = originalMoveSpeedA * newScale;
            moveSpeedD = originalMoveSpeedD * newScale;
            jumpHeight = originalJumpHeight * newScale;
            jumpTime = originalJumpTime * newScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && canSwitchStance)

        {
            switch (jumpStance)
            {

                case 1:
                    Aggreseive(); //Only seeing this now htf did i speel all this wrong but the sameeee goddamnit bruhhhhhhhhh

                    break;

                case 2:
                    Defensive();
                    break;

            }


            jumpStance = (jumpStance == 1) ? 2 : 1;
            lastSwitch = Time.time;
            canSwitchStance = false;

        }



    }

    void Update()
    {

        if (jumpStance == 1)
        {
            Aggreseive();

        }
        if (jumpStance == 2)
        {
            Defensive();
        }
            
        if (canSwitchStance == false && Time.time > lastSwitch + stanceTimer)
        {
            canSwitchStance = true;

            Debug.Log($"StanceSwitch: {canSwitchStance} | Time Left: {lastSwitch + stanceTimer - Time.time}");
        }
    }

    private void StartDioTime()  //YEAAAAA DIO IS EVERYWHEREEEEE
    {
        if (isInDioTime) return; 

        isInDioTime = true;

        moveSpeedA *= TimeManage.Instance.dioTimeScale;
        moveSpeedD *= TimeManage.Instance.dioTimeScale;
        jumpHeight *= TimeManage.Instance.dioTimeScale;
        jumpTime *= TimeManage.Instance.dioTimeScale;

        Debug.Log("DIOOOO!!! U BASTARD!!!!!!!!!"); 

    }

    private void EndDioTime()
    { 
        if(!isInDioTime) return;

        moveSpeedA = originalMoveSpeedA;
        moveSpeedD = originalMoveSpeedD;
        jumpHeight = originalJumpHeight;
        jumpTime = originalJumpTime;

        Debug.Log("DIOOOOOO!!! LETTING ME FREE IS YOUR MISTAKE !!!!!! "); 
    }
    void Aggreseive()
    {
        jumpHeight = 7;
        jumpTime = 7;
        jumpCondition = 2.5f;
        aiMaterial.SetColor("_BaseColor", Color.red);

       

        float currentPosX = transform.position.x;
        float ballPosX = ball.position.x;
        float finalX = Mathf.MoveTowards(currentPosX, ballPosX, moveSpeedA * Time.deltaTime);

        float currentPosZ = transform.position.z;
        float ballposZ = ball.position.z;
        float predictionTimeZ = apredictionZ;
        float ballVelocityZ = ball.linearVelocity.z;
        float predictedBallZ = ball.position.z + (ballVelocityZ * predictionTimeZ);
        float finalZ = Mathf.MoveTowards(currentPosZ, predictedBallZ, moveSpeedA * Time.deltaTime);


        float currentY = transform.position.y + 2;
        float targetY = ball.position.y + 4; 
        float finalY = Mathf.MoveTowards(currentY, targetY, moveSpeedA * Time.deltaTime);

        transform.position = new Vector3(finalX, finalY, finalZ);

    }


    void Defensive()
    {
        jumpHeight = 2;
        jumpTime =  2f;
        jumpCondition = 2.5f;
        


        if (ball.position.y > transform.position.y + jumpCondition)

        {
            StartCoroutine(Jump());
        }
        aiMaterial.SetColor("_BaseColor", Color.blue);

        float currentPosX = transform.position.x;
        float ballPosX = ball.position.x;
        float ballVelocityX = ball.linearVelocity.x;
        float predictionTimeX = dpredictionX;
        float predictedBallX = ball.position.x + (ballVelocityX * predictionTimeX);

        float finalX = Mathf.MoveTowards(currentPosX, predictedBallX, moveSpeedD * Time.deltaTime);

        float currentPosZ = transform.position.z;

        float ballVelocityZ = ball.linearVelocity.z - currentPosZ;
        float predictionTimeZ = dpredictionZ;
        float predictedBallZ = ball.position.z + (ballVelocityZ * predictionTimeZ);
        float finalZ = Mathf.MoveTowards(currentPosZ, predictedBallZ, moveSpeedD * Time.deltaTime);

        transform.position = new Vector3(finalX, 0, finalZ);

    }
    public IEnumerator Jump()
    {
        
        isJumping=true;
        offGround = true;
        float timer = 0;
        Vector3 startPosition = transform.position;

        

        while (timer < jumpTime)
        {
            timer += Time.deltaTime;
            float progress = timer / jumpTime;

            float newY = startPosition.y + (jumpHeight * Mathf.Sin(progress * Mathf.PI));

            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }
        transform.position = new Vector3 (transform.position.x,transform.position.y,transform.position.z); 

     

      
        offGround = false;
        isJumping = false;

        
    }


}
