// Wrote this mostly by myself yayyyy
//Used last semester's scripts for pointers and this video for pointers as well on the 25 July 2025
// https://youtu.be/jvtFUfJ6CP8?si=v7R0Yft9adPUVVS 



using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class stevePlayer : MonoBehaviour
{

    private CharacterController steve;
    public Rigidbody ball;
    
   
   
    public int stance ;
    public float stanceTimer; 
    private float lastSwitch = Mathf.Infinity;
    private Vector3 ballHit;
    public Material aiMaterial;
   
    public bool stanceSwitch;
    private ThrowableObject theBallStuck;

    [Header ("Aggresive")]
        public float moveSpeedA;
    public float apredictionZ;
    public float apredictionX;
    [Header("Defensive")]
    public float moveSpeedD;
    public float dpredictionX;
    public float dpredictionZ;

   // public float jump;

    [Header("Clamp Setting")] //HAhahahaha Im dumb time for NavMESH AGAIN Sat here for 2 hours building a box for this idiot 

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;




    private void Awake()
    {
        steve = GetComponent<CharacterController>();
        theBallStuck = FindFirstObjectByType<ThrowableObject>();
        GetComponent <NavMeshAgent>().enabled = true;
        stance = 1;
        stanceSwitch = false;
        lastSwitch =Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")&& stanceSwitch)
           
        {
            switch (stance) { 
                
                case 1: Aggreseive(); //Only seeing this now htf did i speel all this wrong but the sameeee goddamnit bruhhhhhhhhh
                     
                      break;

                case 2: Defensive();
                  break;

            }


        stance = (stance == 1) ? 2 : 1;
        lastSwitch = Time.time;
        stanceSwitch =false;

        }



    }

    void Update()
    {
       // if (theBallStuck.onMySide ==true)
        //{
          //  Vector3.MoveTowards(transform.position,ball.position,moveSpeedA *Time.deltaTime);
           // Debug.Log("Yes the ball is stuck over there u idiot");
       // }

        if (stance == 1) 
        {
           Aggreseive ();
            
        }
        if (stance == 2)
        {
            Defensive(); 
        }

        if (stanceSwitch == false && Time.time > lastSwitch + stanceTimer)
        {
            stanceSwitch = true;
      
            Debug.Log($"StanceSwitch: {stanceSwitch} | Time Left: {lastSwitch + stanceTimer - Time.time}");
        }
    }

    void Aggreseive()
    {
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

        transform.position = new Vector3(finalX, 0, finalZ);
       
    }
       
         
    void Defensive()
    {
        aiMaterial.SetColor("_BaseColor",Color.blue);

        float currentPosX = transform.position.x;
        float ballPosX = ball.position.x ;
        float ballVelocityX = ball.linearVelocity.x;
        float predictionTimeX = dpredictionX;
        float predictedBallX = ball.position.x + (ballVelocityX * predictionTimeX);

        float finalX = Mathf.MoveTowards(currentPosX, predictedBallX, moveSpeedD * Time.deltaTime);

        float currentPosZ = transform.position.z;

        float ballVelocityZ = ball.linearVelocity.z - currentPosZ;
        float predictionTimeZ = dpredictionZ;
        float predictedBallZ = ball.position.z + (ballVelocityZ * predictionTimeZ);
        float finalZ = Mathf.MoveTowards(currentPosZ, predictedBallZ, moveSpeedD * Time.deltaTime);


        //float currentPosY= transform.position.y;
        //float ballPosY = ball.position.y;
        float finalY = 0;
        //if (ballPosY < 4) 
       // { 
         // finalY = Mathf.MoveTowards(currentPosY,ballPosY, jump *Time.deltaTime);
       // }  Im experimentign with a jump for the ai 
        transform.position = new Vector3(finalX, finalY, finalZ);




        //Reminder to code a ball stuck contingency because the ball gets stuck alot on this phase 

        // update im leaving the green stuff in coz why not but its fixed weeeeeeeeeee
   

    }


}
