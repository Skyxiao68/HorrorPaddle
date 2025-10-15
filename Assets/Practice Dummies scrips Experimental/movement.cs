using System.Collections;
using UnityEngine;

public class UIFishMovement : MonoBehaviour
{
    private RectTransform currentTransform;
    public float minSpeed = 4;
    public float maxSpeed = 10;
    private float speed;
    public RectTransform[] movePoint;
    public float slowDown;
    public float wiggleSpeed;
    public float wiggleAmount;
    public float currentSpeed;
    public float turnSpeed;
    private float baseAngle;
    public bool isStuck;
    public float launch;
    public float overShoot;
    private Vector2 currentpos;
    private int currentTargetIndex;

    void Awake()
    {
        // Get references but don't initialize movement yet
        currentTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // INITIALIZE WHEN OBJECT BECOMES ACTIVE
        StartMovement();
    }

    void OnDisable()
    {
        // CLEAN UP WHEN OBJECT BECOMES INACTIVE
        StopAllCoroutines();
        isStuck = false;
    }

    void StartMovement()
    {
      
        speed = Random.Range(minSpeed, maxSpeed);
        currentSpeed = speed;
        isStuck = false;

        if (movePoint != null && movePoint.Length > 0)
        {
            currentTargetIndex = Random.Range(0, movePoint.Length);
        }
        else
        {
            Debug.LogError("No move points assigned!");
        }
    }

    void Update()
    {
        if (!isStuck && movePoint != null && movePoint.Length > 0)
        {
            MoveFish();
        }
    }

    void MoveFish()
    {
        currentpos = currentTransform.anchoredPosition;

        // Get current target
        RectTransform target = movePoint[currentTargetIndex];
        Vector2 direction = (target.anchoredPosition - currentpos).normalized;
        float distance = Vector2.Distance(currentpos, target.anchoredPosition);

        // Move toward target
        currentTransform.anchoredPosition += direction * currentSpeed * Time.deltaTime;

        // Handle rotation and effects
        HandleRotation(direction);
        HandleScaling(direction);

        // Slow down when close to target
        if (distance < slowDown)
        {
            currentSpeed = Mathf.Lerp(0, speed, distance / slowDown);
        }

        // Change target when reached
        if (distance < 0.5f)
        {
            ChooseNewTarget();
        }

        // Check if stuck
        if (currentTransform.anchoredPosition == currentpos)
        {
            StartCoroutine(UnstuckRoutine(direction));
        }
    }

    void HandleRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        baseAngle = Mathf.LerpAngle(baseAngle, angle, turnSpeed * Time.deltaTime);

        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        transform.rotation = Quaternion.Euler(0, 0, baseAngle + wiggle);
    }

    void HandleScaling(Vector2 direction)
    {
        if (direction.x < 0)
        {
            transform.localScale = new Vector2(1, -1);
        }
        else if (direction.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    void ChooseNewTarget()
    {
        if (movePoint.Length > 1)
        {
            int newTarget;
            do
            {
                newTarget = Random.Range(0, movePoint.Length);
            } while (newTarget == currentTargetIndex && movePoint.Length > 1);

            currentTargetIndex = newTarget;
        }

        speed = Random.Range(minSpeed, maxSpeed);
        currentSpeed = speed;
    }

    IEnumerator UnstuckRoutine(Vector2 direction)
    {
        isStuck = true;
        yield return new WaitForSeconds(10f);

        // Give a push in the current direction
        currentTransform.anchoredPosition += direction * currentSpeed * Time.deltaTime;
        isStuck = false;
    }

    // Public method to manually start/stop movement if needed
    public void SetMovementActive(bool active)
    {
        enabled = active;
        if (active)
        {
            StartMovement();
        }
        else
        {
            StopAllCoroutines();
        }
    }
}