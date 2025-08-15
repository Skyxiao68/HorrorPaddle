using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public float nextTeleportTime;
    public float interval = 3;
    private float teleporterReset;
    public GameObject cam;

   
    void Awake()
    {
       // Vector2 minBounds = Camera.main.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
        //Vector2 maxbounds = Camera.main.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));

       // float randomX = Random.Range(minBounds.x,maxbounds.x);
        //float randomY = Random.Range(minBounds.y,maxbounds.y);


      //  transform.position = new Vector2(randomX, randomY);

        nextTeleportTime = Time.time+interval;
        teleporterReset= nextTeleportTime;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Fish"))
        {
           
            StartCoroutine(Jump());

           

        }
    }

   
    public IEnumerator Jump()
        
        {
        yield return new WaitForSeconds(interval);
        TeleportWithinScreen();
        }

    public void TeleportWithinScreen()
    {

       // Vector2 minBounds = Camera.main.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
       // Vector2 maxbounds = Camera.main.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));
       // float randomX = Random.Range(minBounds.x, maxbounds.x);
        //    float randomY = Random.Range(minBounds.y, maxbounds.y);

            if (Time.time > nextTeleportTime)
                nextTeleportTime = Time.time + interval;

           // transform.position = new Vector2(randomX, randomY);
        nextTeleportTime = teleporterReset;
        
    }
      
    }

   

