using UnityEngine;

public class SimpleEyeTracker : MonoBehaviour
{
    [Header("Tracking Target")]
    public GameObject targetObject;
    public float smoothSpeed = 5f;

    void Update()
    {
        if (targetObject != null)
        {
        
            Vector3 directionToTarget = targetObject.transform.position - transform.position;

           
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

           
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }
}