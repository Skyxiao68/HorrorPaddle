//FIRST PERSON MOVEMENT in Unity - FPS Controller
// Brackeys 
// 23 July 2025
//Version 4
//https://youtu.be/_QajrabyTJc?si=r9QYDLhjzeqO89ci

//FIRST PERSON MOVEMENT in 10 MINUTES - Unity Tutorial
// Dave /GameDevelopement
// 23 July 2025
// Version 4
//https://youtu.be/f473C43s8nE?si=Jjp9O05Qddu9J2Hs

//EVENT SYSTEM TUTORIAL 
//ҹӰUnien 
// 19 September 2025
//Version 2
//https://www.bilibili.com/video/BV1sc41197YZ/?spm_id_from=333.337.search-card.all.click&vd_source=735fcdc7dec66ac9bffaf8eac1a52072

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Diagnostics;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;

    [Header("Debug")]
    public Vector2 inputDirection;
    public float inputRun;
    public float inputDash;
    public float inputDio;

    [Header("Moving")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;
    public float gravityMultiplier = 20f;
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
    public bool isGrounded;

    [Header("Jump")]
    public float jumpHieght = 0.5f;
    public float buttonHoldThreshold = 0.5f;
    private float buttonHoldTimer = 0f;
    private bool dashButtonHeld = false;

    [Header("Dash to Ball")]
    public Transform ball;
    public float skillDistance;
    public float skillCooldown;
    private float skillTimer;
    private float lastTapTime;
    public AudioClip dashTp;

    //public float dioTimeScale = 0.2f;
    // public float dioTimeDuration = 3f;

    [Header("ZaWorldo Lite")]
    private bool isDioActive = false;
    private float dioTimer = 0f;
    private float originalFixedDeltaTime;
    public AudioClip zaWorldo;
    public AudioClip timeMove;
    private AudioSource audioSource;
    public bool tutorial= true;

    [Header("Wall")]
    public float inputWall;
    public GameObject Wall;
    public Transform WallSpawn;
    public float wallCoolDown = 5f;
    public float wallDuration = 5f;
    public float wallMoveDuration;
    private float wallTimer = 0;
    private float wallCoolDownTimer = 0;
    private bool isWallActive = false;
    private GameObject wallInstance;
    private float wallCooldownTimer;
    public AudioClip wallUpSound;
    public AudioClip wallDownSound; 

    private void Awake()
    {
        inputControl = new PlayerInputController();
        CC = gameObject.GetComponent<CharacterController>();
        originalFixedDeltaTime = Time.fixedDeltaTime;

        audioSource = GetComponent<AudioSource>();



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
        inputWall = inputControl.Gameplay.Wall.ReadValue<float>();

        HandleWall();

        HandleZaWorld();

        CurrentMovementSpeed = dir.magnitude;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
        xMove = inputDirection.x * currentSpeed;
        yMove = inputDirection.y * currentSpeed;
        dir = transform.forward * yMove + transform.right * xMove;

        CC.Move(dir * Time.unscaledDeltaTime);

        inputRun = inputControl.Gameplay.Run.ReadValue<float>();

        inputDio = inputControl.Gameplay.ZaWorldo.ReadValue<float>();

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


        velocity.y += GetCurrentGravity() * Time.deltaTime;
        CC.Move(velocity * Time.deltaTime);

        if (isRunning)
        {
            runStam -= stamDeplete * Time.unscaledDeltaTime;
            runStam = Mathf.Max(0, runStam);
        }
        else
        {
            runStam += stamRecover * Time.unscaledDeltaTime;
            runStam = Mathf.Min(maxStam, runStam);
        }

        //inputDash = inputControl.Gameplay.Dash.ReadValue<float>();
        float currentDashInput = inputControl.Gameplay.Dash.ReadValue<float>();

        if (currentDashInput == 1 && dashButtonHeld == false)

        {
            dashButtonHeld = true;
            buttonHoldTimer = 0f;

            if (Time.time - lastTapTime < 0.3f)
            {
                DashToBall();
                lastTapTime = 0;
            }
            else
            {
                lastTapTime = Time.time;
            }
        }

        if (dashButtonHeld) { buttonHoldTimer += Time.unscaledDeltaTime; }

        if (currentDashInput == 0 && dashButtonHeld == true)
        {

            dashButtonHeld = false;

            if (buttonHoldTimer >= buttonHoldThreshold)
            {
                Jump();
            }
            else
            {
                if (Time.time - lastTapTime <= 0.3f)
                    Dash();
            }

        }

    }



    void HandleWall()
    {
        if (tutorial ==true) { return; }

        if (wallCooldownTimer < wallCoolDown)
        {
            wallCoolDownTimer += Time.unscaledDeltaTime;
        }




        if (inputWall == 1 && !isWallActive && wallCoolDownTimer >= wallCoolDown)
        {
            PutWall();
        }
    }

    void PutWall()
    {
        if (WallSpawn == null)
        {
            UnityEngine.Debug.Log("Wall Spawn not Assigned");
        }


        Vector3 startPosition = transform.position + transform.forward * 2f;
        wallInstance = Instantiate(Wall, startPosition, Quaternion.identity);

        StartCoroutine(MoveWallToSpawnPoint());

        isWallActive = true;

        if (wallUpSound != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(wallUpSound);

            StartCoroutine(FadeAudioIn(audioSource, 0.5f));
        }

        wallCoolDownTimer = 0f;

        UnityEngine.Debug.Log("Wall Up");
    }


    IEnumerator MoveWallToSpawnPoint()
    {
        float elapsedTime = 0f;
        Vector3 startPos = wallInstance.transform.position;

        while (elapsedTime < wallMoveDuration)
        {
            wallInstance.transform.position = Vector3.Lerp(startPos, WallSpawn.position, elapsedTime / wallMoveDuration);

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        wallInstance.transform.position = WallSpawn.position;

        yield return new WaitForSeconds(wallDuration);

        WallDisable();
    }

    void WallDisable()
    {
        isWallActive = false;

        if (wallInstance != null)
        {
            Destroy(wallInstance);

           
        }


        if (wallDownSound != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(wallDownSound);

            StartCoroutine(FadeAudioIn(audioSource, 0.5f));
        }

        UnityEngine.Debug.Log("Wall Down");

    }



    void HandleZaWorld()
    {
        if (tutorial == true) { return; }
        if (inputDio == 1 && !isDioActive)
        {
            StartZaWorldo();
        }

        if (isDioActive)
        {
            dioTimer += Time.unscaledDeltaTime;

            if (dioTimer >= TimeManage.Instance.dioTimeDuration)
            {
                EndZaWorldo();

            }

        }

    }

    void StartZaWorldo()
    {
        isDioActive = true;
        dioTimer = 0f;
        Time.timeScale = TimeManage.Instance.dioTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * TimeManage.Instance.dioTimeScale;


        if (zaWorldo != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(zaWorldo);

            StartCoroutine(FadeAudioIn(audioSource, 0.5f));
        }

        EventManage.TriggerEvent("DioTimeStarted");


        UnityEngine.Debug.Log("ZAAA WORLDOOOOOOOO ");
    }

    public void ChangeDioTimeScale(float newScale)
    {
        TimeManage.Instance.SetDioTimeScale(newScale);


        if (isDioActive)
        {
            Time.timeScale = TimeManage.Instance.dioTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime * TimeManage.Instance.dioTimeScale;
        }
    }

    void EndZaWorldo()
    {
        isDioActive = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;

        if (timeMove != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(timeMove);
        }

        EventManage.TriggerEvent("DioTimeEnded");

        UnityEngine.Debug.Log(" Time Begins to Run");

    }

    IEnumerator FadeAudioIn(AudioSource audioSource, float fadeTime)
    {
        float startVolume = 0.2f;
        audioSource.volume = startVolume;

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }


    void DashToBall()
    {
        if (tutorial == true) { return; }

        if (Time.time < skillTimer + skillCooldown) return;



        if (ball == null)
        {
            UnityEngine.Debug.LogError("Ball reference is not set for Skill!");
            return;
        }

        

        Vector3 ballForward = ball.transform.forward;
        Vector3 targetPosition = ball.transform.position + ballForward * skillDistance;

        UnityEngine.Debug.DrawLine(ball.transform.position, targetPosition, Color.blue, 2f);

        GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        marker.transform.position = targetPosition;
        marker.transform.localScale = Vector3.one * 0.5f;
        Destroy(marker, 2f);

        float playerRadius = 0.5f;
        Vector3 checkPosition = targetPosition;
        checkPosition.y += playerHeight / 2f;

       
        if (!Physics.CheckSphere(checkPosition, playerRadius, obsticaleLayers))
        {
           
            StartCoroutine(ExecuteDash(targetPosition));
            skillTimer = Time.time;
            UnityEngine.Debug.Log("DashToBall activated! Player will appear in front of the ball.");
        }
        else
        {
           
            if (FindValidPositionAlongBallDirection(ballForward, out Vector3 validPosition))
            {
                StartCoroutine(ExecuteDash(validPosition));
                skillTimer = Time.time;
                UnityEngine.Debug.Log("DashToBall activated at adjusted position in front of the ball!");
            }
            else
            {
                UnityEngine.Debug.Log("No valid position found in front of the ball. Cannot use Skill");
            }
        }
    }

    
    bool FindValidPositionAlongBallDirection(Vector3 ballDirection, out Vector3 validPosition)
    {
        validPosition = ball.transform.position;
        float maxDistance = 5f; 
        float stepSize = 0.5f; 
        int maxSteps = (int)(maxDistance / stepSize);

        
        for (int i = 1; i <= maxSteps; i++)
        {
            float distance = stepSize * i;
            Vector3 testPosition = ball.transform.position + ballDirection * distance;
            testPosition.y = ball.transform.position.y; 

           
            Vector3 checkPos = testPosition;
            checkPos.y += playerHeight / 2f;

            if (!Physics.CheckSphere(checkPos, 0.5f, obsticaleLayers))
            {
                validPosition = testPosition;

                
                GameObject validMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                validMarker.transform.position = validPosition;
                validMarker.transform.localScale = Vector3.one * 0.3f;
                validMarker.GetComponent<Renderer>().material.color = Color.green;
                Destroy(validMarker, 2f);

                return true;
            }
        }

        return false;
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
            UnityEngine.Debug.Log("Obstacles in the way, cannot dash");
        }
    }

    IEnumerator ExecuteDash(Vector3 targetPosition)
    {
        float dashDuration = 0.2f;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        
        Vector3 lookDirection = (ball.transform.position - targetPosition).normalized;
        lookDirection.y = 0; 
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        Quaternion startRotation = transform.rotation;

        while (elapsedTime < dashDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float fraction = elapsedTime / dashDuration;

           
            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);

           
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fraction);

            if (dashTp != null && elapsedTime < 0.1f)
            {
                audioSource.pitch = 1.2f;
                audioSource.PlayOneShot(dashTp);
            }

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation; 
        dashTimer = Time.time;
    }
    float GetCurrentGravity()
    {
        float currentGravity = gravity;

      
        if ( isRunning)
        {
            currentGravity *= gravityMultiplier;
        }

        return currentGravity;
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
            if (inputDir == Vector3.zero) inputDir = transform.forward;
            else inputDir = transform.TransformDirection(inputDir);

            DashDirection(inputDir);
        }
    }

}





















