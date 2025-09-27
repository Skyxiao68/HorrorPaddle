//FIRST PERSON MOVEMENT in Unity - FPS Controller
// Brackeys 
// 23 July 2025
//Version 4
//https://youtu.be/_QajrabyTJc?si=r9QYDLhjzeqO89ci

//FIRST PERSON MOVEMENT in 10 MINUTES - Unity Tutorial
// Dave /GameDevelopement
// 23 July 2035
// Version 4
//https://youtu.be/f473C43s8nE?si=Jjp9O05Qddu9J2Hs
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class mainCamera : MonoBehaviour
{
    public PlayerInputController inputControl; 

    
    public float mouseSens;
    
    private float mouseX, mouseY; 

    public Transform player;

    public float xRotation;
    public Vector2 inputLook;

    public Slider senSlider;
    internal static object main;

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

    internal Vector3 ScreenToWorldPoint(Vector3 vector3)
    {
        throw new NotImplementedException();
    }
}

