using UnityEngine;

public class bounce : MonoBehaviour
{
    public float bounceForce = 3;
    public float hitForce= 10;
    public Rigidbody ball;
    private Vector3 bouncer;
    private Vector3 hitter;
    public bool canHit;

    private void Awake()
    {
      

        canHit = false;
    }
    private void FixedUpdate()
    {
        bouncer = new Vector3(bounceForce, 0, bounceForce);
        hitter = new Vector3 (hitForce, 0, hitForce);


        if (Input.GetKeyDown(KeyCode.Mouse0) && canHit == true)

        {
            ball.AddForce(hitter);

            Debug.Log("Hitting worls");
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == null) return;
        {
            ball.AddForce(bouncer);
            Debug.Log("I am bouncing");
        }
    }
            //wrote this shi from ma headdddddd

        public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            canHit = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            canHit = false;
        }
    }

}




