using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public GameObject player;
    public GameObject antagonist;
    public GameObject floor;
    public GameObject bat;
    public float cutsceneDuration = 8f;

    private bool cutscenePlaying = false;
    private float cutsceneTimer = 0f;



    [Header("Voice Dialogue Audio")]
    public AudioClip playerLine1;        // 
    public AudioClip antagonistLine1;    //
    public AudioClip antagonistLine2;    // 
    public AudioClip playerLine2;        // 
    public AudioClip antagonistLine3;    // 

    private bool isMovingPlayer = false;
    private Vector3 playerStartPosition;
    private Vector3 playerTargetPosition;
    private float playerMoveDuration = 5f;
    private float playerMoveTimer = 0f;

  
    private bool isRotatingAntagonist = false;
    private Quaternion antagonistStartRotation;
    private Quaternion antagonistTargetRotation;
    private float antagonistRotateDuration = 1.5f;
    private float antagonistRotateTimer = 0f;

  
    private bool isRotatingFloor = false;
    private Quaternion floorStartRotation;
    private Quaternion floorTargetRotation;
    private float floorRotateDuration = 2f;
    private float floorRotateTimer = 0f;

    private bool isBatMoving = false;
    private Vector3 batStartPosition;
    private Vector3 batTargetPosition;
    private float batmoveDuration = 2f;
    private float batMoveTimer = 0f;


    // Dialogue tracking
    private bool playedPlayerLine1 = false;
    private bool playedAntagonistLine1 = false;
    private bool playedAntagonistLine2 = false;
    private bool playedPlayerLine2 = false;
    private bool playedAntagonistLine3 = false;
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



        batStartPosition = bat.transform.position;
        batTargetPosition = batStartPosition + bat.transform.forward * 2f;
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

        if (cutsceneTimer > 12f && !isBatMoving)
        {
            StartBatMovement();
        }
       
        if (cutsceneTimer > 15f && !isRotatingFloor)
        {
            StartFloorRotation();
        }
    }
    // ADD THIS NEW METHOD FOR DIALOGUE TIMING
    void CheckDialogueTiming()
    {
        // Player: "What is this place?" - as they start moving
        if (cutsceneTimer > 1f && !playedPlayerLine1)
        {
            PlayPlayerLine1();
        }

        // Antagonist: "Ah, our guest arrives" - during player movement
        if (cutsceneTimer > 4f && !playedAntagonistLine1)
        {
            PlayAntagonistLine1();
        }

        // Antagonist: "Watch this!" - when bat appears
        if (cutsceneTimer > 12f && !playedAntagonistLine2)
        {
            PlayAntagonistLine2();
        }

        // Player: "What are you doing?!" - reacting to bat
        if (cutsceneTimer > 13f && !playedPlayerLine2)
        {
            PlayPlayerLine2();
        }

        // Antagonist: "Time to drop in!" - before floor collapses
        if (cutsceneTimer > 14.5f && !playedAntagonistLine3)
        {
            PlayAntagonistLine3();
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
        if (isBatMoving)
        {
            UpdateBatMovement();
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

    void StartBatMovement()
    {
        isBatMoving = true;
        batMoveTimer = 0f;
        Debug.Log("bAT IS MOVING");
    }

    void UpdateBatMovement()
    {
        batMoveTimer += Time.deltaTime;
        float progress = batMoveTimer / batmoveDuration;
       // bat.transform.rotation = Quaternion.Lerp (batStartRotation, batTargetRotation, progress);
        bat.transform.position = Vector3.Lerp (batStartPosition, batTargetPosition, progress);
    }
    
    void PlayPlayerLine1()
    {
        playedPlayerLine1 = true;
        PlayAudioClip(playerLine1, player.transform.position);
        Debug.Log("Player: 'What is this place?'");
    }

    void PlayAntagonistLine1()
    {
        playedAntagonistLine1 = true;
        PlayAudioClip(antagonistLine1, antagonist.transform.position);
        Debug.Log("Antagonist: 'Ah, our guest arrives'");
    }

    void PlayAntagonistLine2()
    {
        playedAntagonistLine2 = true;
        PlayAudioClip(antagonistLine2, antagonist.transform.position);
        Debug.Log("Antagonist: 'Watch this!'");
    }

    void PlayPlayerLine2()
    {
        playedPlayerLine2 = true;
        PlayAudioClip(playerLine2, player.transform.position);
        Debug.Log("Player: 'What are you doing?!'");
    }

    void PlayAntagonistLine3()
    {
        playedAntagonistLine3 = true;
        PlayAudioClip(antagonistLine3, antagonist.transform.position);
        Debug.Log("Antagonist: 'Time to drop in!'");
    }

    void PlayAudioClip(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
    void EndCutscene()
    {
        cutscenePlaying = false;
        Debug.Log("Cutscene ended! Loading next scene...");
        SceneManager.LoadSceneAsync(0);
    }
}