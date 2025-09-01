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

    [Header("Debug")]
    public Vector2 inputDirection;
    public float inputRun;
    public float inputDash;

    [Header("Moving")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f; // 改为负值，模拟真实重力
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
        CurrentMovementSpeed = dir.magnitude;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 轻微向下的力确保角色保持在地面上
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

        // 应用重力
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

        inputDash = inputControl.Gameplay.Dash.ReadValue<float>();

        if (inputDash == 1 && Time.time > dashTimer + dashCooldown)
        {
            Vector3 inputDir = new Vector3((xMove), 0f, yMove).normalized;

            if (inputDir == Vector3.zero)
            {
                inputDir = transform.forward;
            }
            else
            {
                inputDir = transform.TransformDirection(inputDir);
            }

            DashDirection(inputDir);
        }
    }

    void DashDirection(Vector3 direction)
    {
        Vector3 dashPosition = transform.position + direction * dashDistance;

        if (!Physics.Raycast(transform.position + Vector3.up * 0.5f, direction, dashDistance, obsticaleLayers))
        {
            StartCoroutine(ExecuteDash(dashPosition));
        }
        else
        {
            Debug.Log("Obstacles in the way, cannot dash");
        }
    }

    IEnumerator ExecuteDash(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.1f);

        if (CC != null)
        {
            CC.enabled = false;
        }

        transform.position = targetPosition;

        if (CC != null)
        {
            CC.enabled = true;
        }

        dashTimer = Time.time;
    }
}






















