using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody rb;
    public CharacterController controller;
    public Transform cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    void Update()
    {
        
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 move =(transform.right* x + transform.forward * z);


       controller.Move(move*moveSpeed*Time.deltaTime);

        transform.rotation = cam.rotation;

    }
}

