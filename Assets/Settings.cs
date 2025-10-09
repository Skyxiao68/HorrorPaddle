using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Settings : MonoBehaviour
{
    public PlayerInputController inputControl;
    public Material settingsMaterial;
    public GameObject settingsMenu;
    public Button back;

    [Header("Cam transition")]
    public Transform camTarget;
    public float transitionDuration = 2.0f;
    public bool isTransitioning = false;

    [Header("Main menu settings")]
    public bool originalStored = false;
    public bool isAtSettingsView = false;
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

        // SIMPLE: Left click returns from settings view
        if (isAtSettingsView && !isTransitioning)
        {
            // Check for left mouse click using new input system
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                ReturnCameraToOriginal();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            settingsMaterial.SetColor("_Color", Color.red);

            // Use your existing menu click to ENTER settings
            float clickValue = inputControl.UI.MenuClick.ReadValue<float>();
            if (clickValue == 1 && !isTransitioning && camTarget != null && !isAtSettingsView)
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

        Vector3 targetPosition = isAtSettingsView ? originalCameraPosition : camTarget.position;
        Quaternion targetRotation = isAtSettingsView ? originalCameraRotation : camTarget.rotation;

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, smoothT);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetRotation, smoothT);

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
        {
            StartCameraTransition();
        }
    }
}