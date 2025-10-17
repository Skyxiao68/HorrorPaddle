using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public PlayerInputController inputControl;
    public Material playMaterial;
    public float click;
    private Camera mainCamera;
    public Animator transiton;
    public ParticleSystem interactParticles;
    public AudioSource clickAudio;
    public AudioClip clickAudioClip; 
    [Header("Cam transition")]
    public Transform camTarget;
    public float transitionDuration = 2.0f;
    public bool canTransition = true;
    public bool isTransitioning = false;
    private float transitionTimer;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private float previousClickValue = 0f;



   

    public void Awake()
    {
       
        inputControl = new PlayerInputController();
       
        playMaterial.SetColor("_Color", Color.white);
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        click = inputControl.UI.MenuClick.ReadValue<float>();

      
        if (isTransitioning)
        {
            HandleCamTransition();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            playMaterial.SetColor("_Color", Color.red);

           
            if (click == 1 && previousClickValue == 0 && canTransition && !isTransitioning)
            {
                Instantiate(interactParticles, transform.position, Quaternion.Euler(-90, 0, 0));
                clickAudio.PlayOneShot(clickAudioClip);
                StartCam();
            }
        }

        previousClickValue = click;
    }

    private void OnTriggerExit(Collider other)
    {
        playMaterial.SetColor("_Color", Color.white);
    }

    void StartCam()
    {
        if (mainCamera != null && camTarget != null)
        {
            // Store original position only once
            originalCameraPosition = mainCamera.transform.position;
            originalCameraRotation = mainCamera.transform.rotation;
            transiton.SetBool("Start",true);
            isTransitioning = true;
            transitionTimer = 0f;
            canTransition = false; 
        }
    }

    void HandleCamTransition()
    {
        
        transitionTimer += Time.deltaTime;
      
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        mainCamera.transform.position = Vector3.Lerp(originalCameraPosition, camTarget.position, smoothT);
        mainCamera.transform.rotation = Quaternion.Lerp(originalCameraRotation, camTarget.rotation, smoothT);

        if (transitionTimer >= transitionDuration)
        {
            isTransitioning = false;
            SceneManager.LoadScene(1); 
        }
    }
}