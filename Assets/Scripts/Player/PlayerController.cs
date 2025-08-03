using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;

    [Header("Debug")]
    public Vector2 inputDirection;
    public float inputRun;

    [Header("Moving")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Running")]
    public float runSpeed = 8f;
    public float runStam = 100f;
    public float maxStam = 100f;
    public float stamDeplete = 20f;
    public float stamRecover = 15f;
    public bool isRunning = false;


    [Header("GroundCHeck")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController CC;
    private float currentSpeed;
    private float xMove, yMove;
    private Vector3 dir;
    private Vector3 velocity;
    private bool isGrounded;


    private void Awake()
    {
        inputControl = new PlayerInputController();
        CC = gameObject.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    void Start()
    {
        currentSpeed = walkSpeed;

    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        xMove = inputDirection.x * currentSpeed;
        yMove = inputDirection.y * currentSpeed;
        dir = transform.forward * yMove + transform.right * xMove;

        CC.Move(dir * Time.deltaTime);

        inputRun = inputControl.Gameplay.Run.ReadValue<float>();


        if (inputRun == 1 && runStam > 20f)
        {
            isRunning = true;
            currentSpeed = runSpeed;
        }

        if (inputRun == 0 || runStam <= 0f)
        {
            isRunning = false;
            currentSpeed = walkSpeed;
        }


        velocity.y += gravity * Time.deltaTime;

        if (isRunning)
        {
            runStam -= stamDeplete * Time.deltaTime;
            runStam = Mathf.Max(0, runStam);
        }
        else
        {
            runStam += stamRecover * Time.deltaTime;
            runStam = Mathf.Min(maxStam, runStam); 
   
        }


    }


}
  
   
       
    
