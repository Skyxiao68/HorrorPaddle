//FIRST PERSON MOVEMENT in Unity - FPS Controller
// Brackeys 
// 23 July 2025
//Version 4
//https://youtu.be/_QajrabyTJc?si=r9QYDLhjzeqO89ci

//FIRST PERSON MOVEMENT in 10 MINUTES - Unity Tutorial
// Dave /GameDevelopement
// 23 July 2035
// Version 4
//https://youtu.be/f473C43s8nE?si=Jjp9O05Qddu9J2Hs

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;
    public GameObject ball; 

    [Header("Debug")]
    public Vector2 inputDirection;
    public float inputRun;
    public float inputDash;
    public float inputSkill; 

    [Header("Moving")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f; 
    public float CurrentMovementSpeed { get; private set; }

    [Header("Running")]
    public float runSpeed = 8f;
    public float runStam = 100f;
    public float maxStam = 100f;
    public float stamDeplete = 20f;
    public float stamRecover = 15f;
    public bool isRunning = false;

    [Header("Dash")]
    public float dashDistance = 3f;
    public float dashCooldown = 3f;
    public float dashSmoother = 5f;
    private float dashTimer;
    public float playerHeight = 2f;
    public LayerMask obsticaleLayers;

    [Header("Skill")]
    public float skillCooldown = 5f;
    public float skillDistance = 2f;
    private float skillTimer;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float groundCheckDistance = 0.2f;

    private CharacterController CC;
    private float currentSpeed;
    private float xMove, yMove;
    private Vector3 dir;
    private Vector3 velocity;
    public bool isGrounded;

    [Header("Jump")]
    public float jumpHieght = 0.5f;
    public float buttonHoldThreshold = 0.5f;
    private float buttonHoldTimer = 0f;
    private bool dashButtonHeld = false;
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

    void FixedUpdate()
    {
        CurrentMovementSpeed = dir.magnitude;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -10f; 
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
        CC.Move(velocity * Time.deltaTime);

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

        //inputDash = inputControl.Gameplay.Dash.ReadValue<float>();
        float currentDashInput = inputControl.Gameplay.Dash.ReadValue<float>();

        if (currentDashInput == 1 && dashButtonHeld == false)
        
        { dashButtonHeld = true;
            buttonHoldTimer = 0f;

        }

            if (dashButtonHeld) {buttonHoldTimer += Time.deltaTime;}

            if (currentDashInput == 0 && dashButtonHeld == true)
            {
                dashButtonHeld = false;
                if (buttonHoldTimer < buttonHoldThreshold)
                {
                    Dash();
                }
                else
                {
                    Jump();
                }
            }

            DashDirection(inputDir);
        }
        inputSkill = inputControl.Gameplay.Skill.ReadValue<float>();
        if ((inputSkill == 1  && Time.time > skillTimer + skillCooldown))
        {
            Skill();
        }



       

       // if (inputDash == 1 && Time.time > dashTimer + dashCooldown)
       // {
         //   Vector3 inputDir = new Vector3((xMove), 0f, yMove).normalized;

           // if (inputDir == Vector3.zero)
            //{
             //   inputDir = transform.forward;
         //   }
         //   else                                                      OLD Dash code combining to make a jump
          //  {
          //      inputDir = transform.TransformDirection(inputDir);
          //  }

          //  DashDirection(inputDir);
       // }
    }

    void DashDirection(Vector3 direction)
    {
        Vector3 dashPosition = transform.position + direction * dashDistance;

        if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, dashDistance, obsticaleLayers))
        {
            StartCoroutine(ExecuteDash(dashPosition, true));
        }
        else
        {
            Debug.Log("Obstacles in the way, cannot dash");
        }
    }

    void Skill()
    {
        if (ball == null)
        {
            Debug.LogError("Ball reference is not set!");
            return;
        
    
        }

        Vector3 ballForward = ball.transform.forward;
        Vector3 targetPosition = ball.transform.position + ballForward * skillDistance;

        // ȷ��Ŀ��λ���ڵ�����
        if (Physics.Raycast(targetPosition + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 3f, groundMask))
        {
            targetPosition = hit.point;
            targetPosition.y += playerHeight / 2f; // ȷ�����վ�ڵ�����
        }

        // ���·�����Ƿ����ϰ���
        Vector3 dashDirection = (targetPosition - transform.position).normalized;
        float dashDistance = Vector3.Distance(transform.position, targetPosition);

        if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, dashDirection, dashDistance, obsticaleLayers))
        {
            StartCoroutine(ExecuteDash(targetPosition, false));
        }
        else
        {
            Debug.Log("Obstacles in the way, cannot dash to ball");
        }
    }






    IEnumerator ExecuteDash(Vector3 targetPosition, bool isNormalDash)
    {
        yield return new WaitForSeconds(0.1f);
        float dashDuration= 0.2f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;


        while (elapsedTime < dashDuration)

        { 
        elapsedTime += Time.deltaTime;

            float fraction = elapsedTime / dashDuration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            yield return null;

        
        }

        if (isNormalDash)
        {
            dashTimer = Time.time;
        }
        else
        {
            skillTimer = Time.time;
        }
        transform.position = targetPosition;
        dashTimer = Time.time;
    }
    void Jump()
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(2 * Mathf.Abs(gravity) * jumpHieght);
            }
        }
        void Dash()
        {
            if (Time.time > dashTimer + dashCooldown)
            {
                Vector3 inputDir = new Vector3((xMove), 0, yMove).normalized;
                if (inputDir==Vector3.zero)inputDir = transform.forward;
                else inputDir = transform.TransformDirection(inputDir);

                DashDirection(inputDir);
            }
        }

}


 



















