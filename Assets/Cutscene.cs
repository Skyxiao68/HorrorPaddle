using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public GameObject player;
    public GameObject antagonist;
    public GameObject floor;
    public float cutsceneDuration = 8f;

    private bool cutscenePlaying = false;
    private float cutsceneTimer = 0f;

    // Player movement variables
    private bool isMovingPlayer = false;
    private Vector3 playerStartPosition;
    private Vector3 playerTargetPosition;
    private float playerMoveDuration = 5f;
    private float playerMoveTimer = 0f;

    // Antagonist rotation variables
    private bool isRotatingAntagonist = false;
    private Quaternion antagonistStartRotation;
    private Quaternion antagonistTargetRotation;
    private float antagonistRotateDuration = 1.5f;
    private float antagonistRotateTimer = 0f;

    // Floor rotation variables
    private bool isRotatingFloor = false;
    private Quaternion floorStartRotation;
    private Quaternion floorTargetRotation;
    private float floorRotateDuration = 2f;
    private float floorRotateTimer = 0f;

    void Start()
    {
        SetupCutscenePositions();
        StartCutscene();
    }

    void SetupCutscenePositions()
    {
      
        playerStartPosition = player.transform.position;
        playerTargetPosition = playerStartPosition + player.transform.forward * 2f;

       
        antagonistStartRotation = antagonist.transform.rotation;
        antagonistTargetRotation = antagonistStartRotation * Quaternion.Euler(0, 180, 0);

        floorStartRotation = floor.transform.rotation;
        floorTargetRotation = floorStartRotation * Quaternion.Euler(0, 0, 90f);
    }

    void StartCutscene()
    {
        cutscenePlaying = true;
        cutsceneTimer = 0f;
        Debug.Log("Cutscene started!");
    }

    void Update()
    {
        if (cutscenePlaying)
        {
            cutsceneTimer += Time.deltaTime;
            CheckCutsceneEvents();
            UpdateSmoothMovements();

            if (cutsceneTimer > cutsceneDuration)
            {
                EndCutscene();
            }
        }
    }

    void CheckCutsceneEvents()
    {
     
        if (cutsceneTimer > 1f && !isMovingPlayer)
        {
            StartPlayerMovement();
        }

      
        if (cutsceneTimer > 8f && !isRotatingAntagonist)
        {
            StartAntagonistRotation();
        }

       
        if (cutsceneTimer > 15f && !isRotatingFloor)
        {
            StartFloorRotation();
        }
    }

    void UpdateSmoothMovements()
    {
        if (isMovingPlayer)
        {
            UpdatePlayerMovement();
        }

        if (isRotatingAntagonist)
        {
            UpdateAntagonistRotation();
        }

        if (isRotatingFloor)
        {
            UpdateFloorRotation();
        }
    }

    void StartPlayerMovement()
    {
        isMovingPlayer = true;
        playerMoveTimer = 0f;
        Debug.Log("Player moving forward smoothly");
    }

    void UpdatePlayerMovement()
    {
        playerMoveTimer += Time.deltaTime;
        float progress = playerMoveTimer / playerMoveDuration;
        player.transform.position = Vector3.Lerp(playerStartPosition, playerTargetPosition, progress);
    }

    void StartAntagonistRotation()
    {
        isRotatingAntagonist = true;
        antagonistRotateTimer = 0f;
        Debug.Log("Antagonist turning around smoothly");
    }

    void UpdateAntagonistRotation()
    {
        antagonistRotateTimer += Time.deltaTime;
        float progress = antagonistRotateTimer / antagonistRotateDuration;
        antagonist.transform.rotation = Quaternion.Lerp(antagonistStartRotation, antagonistTargetRotation, progress);
    }

    void StartFloorRotation()
    {
        isRotatingFloor = true;
        floorRotateTimer = 0f;
        Debug.Log("Floor rotating smoothly");
    }

    void UpdateFloorRotation()
    {
        floorRotateTimer += Time.deltaTime;
        float progress = floorRotateTimer / floorRotateDuration;
        floor.transform.rotation = Quaternion.Lerp(floorStartRotation, floorTargetRotation, progress);
    }

    void EndCutscene()
    {
        cutscenePlaying = false;
        Debug.Log("Cutscene ended! Loading next scene...");
        SceneManager.LoadSceneAsync(0);
    }
}