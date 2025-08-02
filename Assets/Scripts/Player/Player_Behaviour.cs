using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Behaviour : MonoBehaviour
{
    [Header("Moving")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("GroundCHeck")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Vector3 dir;
    private Vector3 velocity; 
    private bool isGrounded;
    private float currentSpeed;
    private float xMove, yMove; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = walkSpeed;
        
    }

    void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

      
         xMove = Input.GetAxis("Horizontal") * walkSpeed;
         yMove = Input.GetAxis("Vertical") * walkSpeed;
        
        dir = transform.forward * yMove + transform.right * xMove; 
      
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
        }

        
        controller.Move(dir * Time.deltaTime);

       
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("He si jumpingt");
        }

        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}