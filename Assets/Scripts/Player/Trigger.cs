using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ThrowableObject : MonoBehaviour
{
    public float throwForce ;
    public float throwHeight;
    public bool throwToTarget = false; //Aight gonn cook here lol this bool is always going to sit at false ma duddeeeeeeeeee 
    public float launchAngle;
    public Transform target;


    public float hitDistance;


    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, -14f, 0);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowTrigger"))
        {

            throwToTarget = true;

            if (throwToTarget && Input.GetKeyDown(KeyCode.Mouse0) == true)
            {
                ThrowObjectToTarget(target.position);
                Debug.Log("Target in Aight");
            }
            else
            {
                ThrowObject();
            }
        }
    }

    private void Update()
    {
        if (throwToTarget && Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            ThrowObjectToTarget(target.position);
            Debug.Log("Target in Aight");
        }
    }

    private void OnTriggerExit()
    {
        throwToTarget = false; 
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

        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPosition, launchAngle);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float angle)
    {
        float gravity = Physics.gravity.magnitude;
        float horizontalDistance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));
        float effectiveDistance = Mathf.Min(horizontalDistance, hitDistance);
        float velocity = effectiveDistance/ (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);
         velocity = Mathf.Clamp(velocity,5f, 30f); //Come back here for ball launch speeds if shit gets slow 
        Debug.Log($"Raw Distance: {horizontalDistance} | Capped Distance: {effectiveDistance}");




        Vector3 direction = end - start;
        direction.y = 0;
        direction.Normalize();

        Vector3 launchVelocity = direction * velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        launchVelocity.y = velocity * Mathf.Sin(angle * Mathf.Deg2Rad);

        launchVelocity = launchVelocity.normalized * Mathf.Clamp(velocity, 5f, 30f);
        Debug.Log($"Calculated Velocity: {velocity} | From Distance: {horizontalDistance} | Angle: {angle}°");
        return launchVelocity;
    }

  
}
