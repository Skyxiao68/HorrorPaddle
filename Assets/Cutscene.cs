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
    public AudioClip playerLine1;        // AJ- Well well well , if it isnt the showman thats the bastard who stole my body and i would like it back.
    public AudioClip antagonistLine1;    //  Des-if you want it then you are going to have to come and take it
    public AudioClip antagonistLine2;    //  but i dont really feel like it today
    public AudioClip playerLine2;        // Aj - Wait what the hell do you mean you do not feel like it, you will face me you bastard we beat your deadbeat henchman
    public AudioClip antagonistLine3;    // Des- Well you see this is my game after all , and i make the rules, or did this dumbass here not read the fineprint, speaking of which you belong to me 
    public AudioClip playerline3;        // Aj - Nooooo you piece of shit i demand a fair match, you asshole. 
    public AudioClip antagonistLine4;    //Des - Well you see it is technically appropriate, this asshole standing in front of me has not beat my henchman on their first try or did you think those countless retries were free and it is clear that they do not apprecitate my work nor are they a mentally viable specimen, so why should they face me. Off you go as you fall in your endless self loathing void, dont even try to escape hahahahahahahah.

    public AudioClip drumRoll;
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
    private bool playedPlayerLine3 = false;
    private bool playedAntagonistLine4 = false;
    private bool playedDrumRoll = false;
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
            CheckDialogueTiming();
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

      
        if (cutsceneTimer > 10f && !isRotatingAntagonist)
        {
            StartAntagonistRotation();
        }

        if (cutsceneTimer > 32f && !isBatMoving)
        {
            StartBatMovement();
        }
       
        if (cutsceneTimer > 69f && !isRotatingFloor)
        {
            StartFloorRotation();
        }
    }
  
    void CheckDialogueTiming()
    {
       
        if (cutsceneTimer > 1f && !playedPlayerLine1)
        {
            PlayPlayerLine1();
        }

      
        if (cutsceneTimer > 8f && !playedAntagonistLine1)
        {
            PlayAntagonistLine1();
        }
        if (cutsceneTimer>  8F  && !playedDrumRoll)
        {
            PlayTheDrums();
        }
        
        if (cutsceneTimer > 12f && !playedAntagonistLine2)
        {
            PlayAntagonistLine2();
        }

       
        if (cutsceneTimer > 14f && !playedPlayerLine2)
        {
            PlayPlayerLine2();
        }

     
        if (cutsceneTimer > 22f && !playedAntagonistLine3)
        {
            PlayAntagonistLine3();
        }

        if (cutsceneTimer > 33f && ! playedPlayerLine3)
        {
            PlayPlayerLine3();
        }

        if (cutsceneTimer > 39f && !playedAntagonistLine4)
        {
           PlayAntagonistLine4();
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
       
    }

    void PlayAntagonistLine1()
    {
        playedAntagonistLine1 = true;
        PlayAudioClip(antagonistLine1, antagonist.transform.position);
      
    }

    void PlayTheDrums()
    {
        playedDrumRoll = true;
        PlayAudioClip(drumRoll, player.transform.position); 
      
    }
    void PlayAntagonistLine2()
    {
        playedAntagonistLine2 = true;


        PlayAudioClip(antagonistLine2, antagonist.transform.position);
       
       
    }

    void PlayPlayerLine2()
    {
        playedPlayerLine2 = true;
        PlayAudioClip(playerLine2, player.transform.position);
       
    }

    void PlayAntagonistLine3()
    {
        playedAntagonistLine3 = true;
        PlayAudioClip(antagonistLine3, antagonist.transform.position);
       
    }

    void PlayPlayerLine3()
    {
        playedPlayerLine3 = true;
        PlayAudioClip (playerline3 , player.transform.position);
        
    }

    void PlayAntagonistLine4()
    {
        playedAntagonistLine4 = true;
        PlayAudioClip (antagonistLine4, antagonist.transform.position);
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