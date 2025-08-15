using UnityEngine;

public class practiceWalls : MonoBehaviour
{
    public Rigidbody player;
    public ParticleSystem playerParts;
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Ball"))
        {
            Instantiate(playerParts,player.transform);
        } 
    }
}
