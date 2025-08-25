using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class tutpaddle : MonoBehaviour
{
    public GameObject playerBat;
    public float spinSpeed;
    public TextMeshProUGUI batText; 
    void Start()
    {
        playerBat.SetActive(false);
    }

   
    void Update()
    {
        transform.Rotate (spinSpeed,0,spinSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBat.SetActive (true);

            Destroy(this);
        }
        ;
    }
}
