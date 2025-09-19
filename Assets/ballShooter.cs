using System.Collections;
using UnityEngine;

public class BallShooter : MonoBehaviour
{
  public GameObject ball;
    public Transform firePoint;
    public float throwForce;
    public float ballLifeTime;
    public float shootInterval;
  private bool canSpawnBall;
    private bool isShooting;

    private void Start()
    {
        StartCoroutine(SpawningBall());
    }

    private IEnumerator SpawningBall()
    {
       
        while (true)
        {
            GameObject newBall = Instantiate(ball, firePoint.position, firePoint.rotation);
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * throwForce, ForceMode.Impulse);
            Destroy(newBall, ballLifeTime);

            yield return new WaitForSeconds(shootInterval);

          

        }

       

    }


}
