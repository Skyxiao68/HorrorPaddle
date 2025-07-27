using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ThrowableObject : MonoBehaviour
{
    public float throwForce = 10f;
    public float throwHeight = 2f;
    public bool throwToTarget = false;
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowTrigger"))
        {
            if (throwToTarget && target != null)
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

        Vector3 velocity = CalculateLaunchVelocity(transform.position, targetPosition, 45f);
        rb.linearVelocity = velocity;
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 end, float angle)
    {
        float gravity = Physics.gravity.magnitude;
        float horizontalDistance = Vector3.Distance(new Vector3(start.x, 0, start.z), new Vector3(end.x, 0, end.z));

        float velocity = horizontalDistance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

        Vector3 direction = end - start;
        direction.y = 0;
        direction.Normalize();

        Vector3 launchVelocity = direction * velocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        launchVelocity.y = velocity * Mathf.Sin(angle * Mathf.Deg2Rad);

        return launchVelocity;
    }
}