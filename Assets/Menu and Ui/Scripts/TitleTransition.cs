using UnityEngine;

public class TitleTransition : MonoBehaviour
{
    public PlayerInputController inputControl;
    private Camera mainCam;
    public float click;

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
        mainCam = Camera.main;

        if (mainCam != null)
        {
            originalCameraPosition = mainCam.transform.position;
            originalCameraRotation = mainCam.transform.rotation;
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
        click = inputControl.UI.MenuClick.ReadValue<float>();

        if (canTransition && click == 1 && previousClickValue == 0 && !isTransitioning)
        {
            StartCameraTransition();
        }

        previousClickValue = click;

        if (isTransitioning)
        {
            CamMove();
        }
    }

    private void StartCameraTransition()
    {
        if (mainCam != null && camTarget != null)
        {
            isTransitioning = true;
            transitionTimer = 0f;
            canTransition = false; 
        }
    }

    private void CamMove()
    {
        transitionTimer += Time.deltaTime;
        float t = Mathf.Clamp01(transitionTimer / transitionDuration);
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        mainCam.transform.position = Vector3.Lerp(originalCameraPosition, camTarget.position, smoothT);
        mainCam.transform.rotation = Quaternion.Lerp(originalCameraRotation, camTarget.rotation, smoothT);

        if (transitionTimer >= transitionDuration)
        {
            isTransitioning = false;

        }
    }
}