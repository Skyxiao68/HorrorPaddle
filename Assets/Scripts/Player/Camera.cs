using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Camera : MonoBehaviour
{
    public PlayerInputController inputControl; 

    
    public float mouseSens;
    
    private float mouseX, mouseY; 

    public Transform player;

    public float xRotation;
    public Vector2 inputLook;

    public Slider senSlider;

    public void Awake()
    {
        inputControl = new PlayerInputController(); 
    }

    public void OnEnable()
    {
        inputControl.Enable();
    }

    public void OnDisable()
    {
        inputControl.Disable();
    }

    public void Start()
{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        senSlider.value = mouseSens;


}
    private void Update()
    {
        inputLook = inputControl.Gameplay.Look.ReadValue<Vector2>(); 

        mouseX = inputLook.x * Time.deltaTime * mouseSens;
        mouseY = inputLook.y * Time.deltaTime * mouseSens;
        player.Rotate(Vector3.up * mouseX);
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 
        
        
        transform.localRotation= Quaternion.Euler(xRotation,0 ,0);
       
    }
    public void AdjustSens(float newSpeed)
    {
        mouseSens = newSpeed * 18;
    }

}

