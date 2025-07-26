using UnityEngine;

    public class net : MonoBehaviour
{
    
      void Start()
    {
        GetComponent<stevePlayer>(onMySide);
    }

    public void OnTriggerEnter(Collider other)
    { if (other.CompareTag("Ball"))
            onMySide = true;
    }

}

  
    
