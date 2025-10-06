// Mouse Cursor to Move 3D object. 
//https://www.bilibili.com/video/BV1Jp4y1y71C/?spm_id_from=333.337.search-card.all.click&vd_source=735fcdc7dec66ac9bffaf8eac1a52072


using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;

public class menuNav : MonoBehaviour
{
    public PlayerInputController inputControl;
    public float mouseSens = 1f;
    public float gamepadSens = 10f;
    public Transform cursor;

    
    public enum MovementPlane { XZ, XY }
    public MovementPlane movementPlane = MovementPlane.XZ;

    public float cursorHeight = 0f;


    [Header("Input Detection Settings")]
    public bool autoDetectInputDevice = true;
    public float inputDetectionInterval = 0.5f;

  
    public enum InputDeviceType { MouseKeyboard, Gamepad }
    [SerializeField] private InputDeviceType currentInputDevice = InputDeviceType.MouseKeyboard;

    private Camera mainCamera;
    private Vector2 navigateInput;
    private Vector2 mouseInput;
    private Vector3 currentCursorPosition;

   
    private Vector2 virtualScreenPos;
    private Coroutine inputDetectionCoroutine;

    private void Awake()
    {
        inputControl = new PlayerInputController();
        mainCamera = Camera.main;
    }

    public void OnEnable()
    {
        inputControl.Enable();

        
        if (autoDetectInputDevice)
        {
            inputDetectionCoroutine = StartCoroutine(DetectInputDevice());
        }

       
        UpdateCursorForCurrentDevice();
    }

    public void OnDisable()
    {
        inputControl.Disable();

       
        if (inputDetectionCoroutine != null)
        {
            StopCoroutine(inputDetectionCoroutine);
        }
    }

    void Start()
    {
        if (cursor == null)
        {
            Debug.LogError("Cursor transform is not assigned!");
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

     
        if (cursor != null)
        {
            currentCursorPosition = cursor.position;
         
            virtualScreenPos = mainCamera.WorldToScreenPoint(currentCursorPosition);
        }
    }

    private void Update()
    {
      
        mouseInput = inputControl.UI.Point.ReadValue<Vector2>();

      
        navigateInput = inputControl.UI.Navigate.ReadValue<Vector2>();

        
        if (currentInputDevice == InputDeviceType.MouseKeyboard)
        {
           
            currentCursorPosition = ScreenToWorldPosition(mouseInput);
        }
        else 
        {
           
            UpdateCursorWithNavigation();
        }

     
        if (cursor != null)
        {
            cursor.position = currentCursorPosition;
        }
    }

    private void UpdateCursorWithNavigation()
    {
        
        virtualScreenPos += navigateInput * gamepadSens * Time.deltaTime;

        
        virtualScreenPos.x = Mathf.Clamp(virtualScreenPos.x, 0, Screen.width);
        virtualScreenPos.y = Mathf.Clamp(virtualScreenPos.y, 0, Screen.height);

      
        currentCursorPosition = ScreenToWorldPosition(virtualScreenPos);
    }

    private Vector3 ScreenToWorldPosition(Vector2 screenPosition)
    {
        if (movementPlane == MovementPlane.XZ)
        {
           
            Plane plane = new Plane(Vector3.up, new Vector3(0, cursorHeight, 0));
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
        }
        else 
        {
           
            float zDepth = cursor != null ? cursor.position.z : 0;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDepth));
            return worldPos;
        }

     
        return currentCursorPosition;
    }

    
    private IEnumerator DetectInputDevice()
    {
        while (true)
        {
           
            bool gamepadInputDetected = false;

           
            foreach (var gamepad in Gamepad.all)
            {
                if (gamepad.leftStick.ReadValue().sqrMagnitude > 0.01f ||
                    gamepad.rightStick.ReadValue().sqrMagnitude > 0.01f ||
                    gamepad.dpad.ReadValue().sqrMagnitude > 0.01f ||
                    gamepad.buttonSouth.isPressed ||
                    gamepad.buttonEast.isPressed ||
                    gamepad.buttonWest.isPressed ||
                    gamepad.buttonNorth.isPressed)
                {
                    gamepadInputDetected = true;
                    break;
                }
            }

        
            bool keyboardInputDetected = navigateInput.sqrMagnitude > 0.01f;

           
            bool mouseInputDetected = mouseInput.sqrMagnitude > 0.01f ||
                                     Mouse.current.delta.ReadValue().sqrMagnitude > 0.01f;

            InputDeviceType newDeviceType = currentInputDevice;

            if (gamepadInputDetected)
            {
                newDeviceType = InputDeviceType.Gamepad;
            }
            else if (mouseInputDetected || keyboardInputDetected)
            {
                newDeviceType = InputDeviceType.MouseKeyboard;
            }

            
            if (newDeviceType != currentInputDevice)
            {
                currentInputDevice = newDeviceType;
                UpdateCursorForCurrentDevice();
            }

            yield return new WaitForSeconds(inputDetectionInterval);
        }
    }

   
    private void UpdateCursorForCurrentDevice()
    {
        if (currentInputDevice == InputDeviceType.MouseKeyboard)
        {
          
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            
            virtualScreenPos = mouseInput;
        }
        else 
        {
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
           
        }
    }

   
    public void ResetCursorPosition(Vector3 newPosition)
    {
        currentCursorPosition = newPosition;
        if (cursor != null)
        {
            cursor.position = newPosition;
        }
        virtualScreenPos = mainCamera.WorldToScreenPoint(newPosition);
    }


    public Vector3 GetCursorPosition()
    {
        return currentCursorPosition;
    }

  
    public Vector2 GetCursorScreenPosition()
    {
        return currentInputDevice == InputDeviceType.MouseKeyboard ? mouseInput : virtualScreenPos;
    }

    
    public InputDeviceType GetCurrentInputDevice()
    {
        return currentInputDevice;
    }

    public void SetInputDeviceType(InputDeviceType deviceType)
    {
        currentInputDevice = deviceType;
        UpdateCursorForCurrentDevice();
    }

    
    public void SetAutoDetectEnabled(bool enabled)
    {
        autoDetectInputDevice = enabled;

        if (enabled && inputDetectionCoroutine == null)
        {
            inputDetectionCoroutine = StartCoroutine(DetectInputDevice());
        }
        else if (!enabled && inputDetectionCoroutine != null)
        {
            StopCoroutine(inputDetectionCoroutine);
            inputDetectionCoroutine = null;
        }
    }
}