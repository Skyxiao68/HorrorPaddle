using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    void Update()
    {
        
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 move = new Vector3 (x,0,z); 

        rb.MovePosition(transform.position +move * moveSpeed *Time.deltaTime);
    }
}

