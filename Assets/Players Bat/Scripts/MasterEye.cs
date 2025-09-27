
using UnityEngine;
using UnityEngine.InputSystem;


public class MasterEye : MonoBehaviour


{
    Camera cam;
    public PlayerInputController inputControl;
    private void Start()
    {
        inputControl=GetComponent<PlayerInputController>();
        cam = Camera.main;
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
        Vector2 mousePos = inputControl.Gameplay.Look.ReadValue<Vector2>();
        transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5));
    }


}