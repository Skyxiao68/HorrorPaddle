using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float minSpeed = 4;
    public float maxSpeed = 10;
    private float speed;
    public Transform movePoint;
    public float slowDown;
    public float wiggleSpeed;
    public float wiggleAmount;
    public float currentSpeed;
    public float turnSpeed;
    private float baseAngle;
    public bool isStuck;
    public float launch;
    public float overShoot;
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        baseAngle = transform.eulerAngles.z;

        speed = Random.Range(minSpeed, maxSpeed);

        currentSpeed = speed;
        isStuck = false;

       
        
    }

   
    void FixedUpdate()
    {
      
       
        Vector2 direction= ((Vector2)movePoint.position - rb.position).normalized; 
        float distance = Vector2.Distance(rb.position, movePoint.position);


        rb.linearVelocity = direction * speed;
     
        Vector2.MoveTowards(rb.position, direction, speed);
      
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
        
        baseAngle = Mathf.LerpAngle(baseAngle, angle,turnSpeed*Time.deltaTime);

  float wiggle = Mathf.Sin(Time.time *wiggleSpeed)*wiggleAmount;

        transform.rotation=Quaternion.Euler(0,0,baseAngle+wiggle);
        
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);


        if (direction.x < 0)
        {
            transform.localScale = new Vector2(1,-1); 
        }
        if (direction.x > 180) 
        { 
            transform.localScale = new Vector2 (-1,1);
        }
        if (direction.y > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        if (direction.y < 189)
        {
            transform.localScale = new Vector2(1, -1);
        }

        else
        { transform.localScale = new Vector2(1, 1); }
        if (distance < slowDown)
        {
            currentSpeed = Mathf.Lerp(0, speed, distance / slowDown);
        }

        if (distance < 0.5)
        {
            rb.linearVelocity = Vector2.zero;
            speed = Random.Range(minSpeed,maxSpeed);
        }else
        {
            rb.linearVelocity = Vector2.Lerp (rb.linearVelocity, Vector2.zero,Time.deltaTime*5f);
        }

        if (rb.linearVelocity == Vector2.zero)
        {
            StartCoroutine(Unstuck());
            isStuck = true;
        }
        if (Input.GetMouseButtonDown(0))
        {  
           
        }

        IEnumerator Unstuck()
        {
            yield return new WaitForSeconds(10);

            rb.linearVelocity = direction * currentSpeed;
            isStuck = false;
        }


        
    
    }
}
