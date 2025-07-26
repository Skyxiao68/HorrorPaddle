using Unity.VisualScripting;
using UnityEngine;

public class stevePlayer : MonoBehaviour
{

    private CharacterController steve;
    public Rigidbody ball;
    public float moveSpeed;

    public float hitBall;

    private Vector3 ballHit;
    public float posZ;
    private void Awake()
    {
        steve = GetComponent<CharacterController>();
    }

    void Update()
    {

        ballHit = new Vector3 (hitBall,0, hitBall);

        float currentPos = transform.position.x;
        float ballPos = ball.position.x;

        float newPos = Mathf.MoveTowards ( currentPos, ballPos, moveSpeed*Time.deltaTime);

        transform.position = new Vector3(newPos, 3, posZ);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ball.AddForce(ballHit *hitBall, ForceMode.Impulse);
        }
    }
}
