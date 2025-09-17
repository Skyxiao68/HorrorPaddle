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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawnBall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canSpawnBall = false;   
    }

    private void Update()
    {
        if (canSpawnBall == true && isShooting == false)
        {
            StartCoroutine(SpawningBall());
        }
    }

   private IEnumerator SpawningBall()
    {
        isShooting = true;

        // This loop will run as long as canShoot is true
        while (canSpawnBall)
        {
            GameObject newBall = Instantiate(ball, firePoint.position, firePoint.rotation);
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * throwForce, ForceMode.Impulse);
            Destroy(newBall, ballLifeTime);

            yield return new WaitForSeconds(shootInterval);

          

        }

        isShooting = false;

    }


}
