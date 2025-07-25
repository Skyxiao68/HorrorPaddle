using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float gravity = -9.82f;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    private Rigidbody rb;
    public CharacterController controller;
    public Transform cam;


    Vector3 velocity;
    public bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y <0 )
        {

            velocity.y = -2f;
        }
        
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 move =(transform.right* x + transform.forward * z);
        moveSpeed = Mathf.Lerp(z, moveSpeed, moveSpeed);

       controller.Move(move*moveSpeed*Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move (velocity*Time.deltaTime);
        transform.rotation = cam.rotation;

    }
}

