using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public PlayerInputController inputControl;
    public Material settingsMaterial;
    public float click;
    public GameObject settingsMenu;
    public Button back;

    [Header("Cam transition")]
    public Transform camTarget;
    public float transitionDuration = 2.0f;
    public bool isTransitioning = false;

    [Header("Main menu settings")]
    public bool originalStored=false; 
    private bool isAtSettingsView=false;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private Transform originalCamPoint;
  
    private float transitionTimer;
    private Camera mainCamera;

    public void Awake()
    {
        settingsMaterial.SetColor("_Color", Color.white);
        inputControl = new PlayerInputController();
        mainCamera = Camera.main;
        settingsMenu.SetActive(false);


    }
    private void Start()
    {
      
        if (back != null)
        {
            back.onClick.AddListener(ReturnCameraToOriginal);
        }
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
        if (isTransitioning)
        {
            HandleCameraTransition();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        click = inputControl.UI.MenuClick.ReadValue<float>();
        if (other.gameObject.CompareTag("Ball"))
        {
            settingsMaterial.SetColor("_Color", Color.red);
            if (click == 1&& !isTransitioning&& camTarget !=null&& !isAtSettingsView)
            {
                StartCameraTransition();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        settingsMaterial.SetColor("_Color", Color.white);
    }

    private void StartCameraTransition()
    {
        if (!originalStored)
        {
            originalCameraRotation = mainCamera.transform.rotation;

         originalCamPoint = mainCamera.transform.parent;
            originalCameraPosition = mainCamera.transform.position;
            originalStored = true;
        }
      
       

        isTransitioning = true;
        transitionTimer = 0f;
    }
    private void HandleCameraTransition()
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);

        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,isAtSettingsView?originalCameraPosition :camTarget.position,smoothT);

        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, isAtSettingsView ? originalCameraRotation : camTarget.rotation, smoothT);

        if (transitionTimer >= transitionDuration)
        {
            isTransitioning = false;
            isAtSettingsView = !isAtSettingsView;
            settingsMenu.SetActive(isAtSettingsView);
        }
    }
    public void ReturnCameraToOriginal()
    {
      
         if (!isTransitioning && isAtSettingsView)

            StartCameraTransition();
            settingsMenu.SetActive(false);
    }
}
