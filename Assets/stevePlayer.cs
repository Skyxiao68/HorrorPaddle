using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class stevePlayer : MonoBehaviour
{

    private CharacterController steve;
    public Rigidbody ball;
    public float moveSpeed;
    public float prediction;
    public float hitBall;

    private Vector3 ballHit;

    public bool onMySide; //Might use later man i wanted to make them have boundaries but i seem to have done that with a nav mesh so i think we good 

    [Header("Clamp Setting")] //HAhahahaha Im dumb time for NavMESH AGAIN Sat here for 2 hours building a box for this idiot 

    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;




    private void Awake()
    {
        steve = GetComponent<CharacterController>();
        onMySide = true;
        GetComponent <NavMeshAgent>().enabled = true;
    }

    void Update()
    {





              ballHit = new Vector3 (hitBall,0, hitBall);

        

            float currentPosX = transform.position.x;
            float ballPosX = ball.position.x;

            float finalX = Mathf.MoveTowards(currentPosX, ballPosX, moveSpeed * Time.deltaTime);

            float currentPosZ = transform.position.z;
         
            float ballVelocity = ball.linearVelocity.z;
            float predictionTime = prediction;
            float predictedBall = ball.position.z +(ballVelocity * predictionTime);
            float finalZ = Mathf.MoveTowards(currentPosZ, predictedBall, moveSpeed * Time.deltaTime);

            transform.position = new Vector3(finalX, 0, finalZ);
        

        if (onMySide == false)
        
        {

            float patrolX = Random.Range(minX, maxX);
            float patrolz = Random.Range(minZ, maxZ);
           
            transform.position = new Vector3 (patrolX,3,patrolz);


        
        
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            //wball.AddForce(ballHit *hitBall, ForceMode.Impulse);
        }
    }
}
