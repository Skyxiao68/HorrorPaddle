// Mouse Cursor to Move 3D object. 
//https://www.bilibili.com/video/BV1Jp4y1y71C/?spm_id_from=333.337.search-card.all.click&vd_source=735fcdc7dec66ac9bffaf8eac1a52072

using UnityEngine;
using UnityEngine.InputSystem;

public class menuNav : MonoBehaviour
{
    public PlayerInputController inputControl;
    public float mouseSens = 1f;
    public Transform cursor;
    public Vector2 mousePos;

   
    public enum MovementPlane { XZ, XY }
    public MovementPlane movementPlane = MovementPlane.XZ;

   
    public float cursorHeight = 0f;

    private Camera mainCamera;

    private void Awake()
    {
        inputControl = new PlayerInputController();
        mainCamera = Camera.main;
    }

    public void OnEnable()
    {
        inputControl.Enable();
    }

    public void OnDisable()
    {
        inputControl.Disable();
    }

    void Start()
    {
        Cursor.visible = true;


        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        mousePos = inputControl.UI.Point.ReadValue<Vector2>();

        if (cursor != null && mainCamera != null)
        {
           
            Vector3 worldPosition = ScreenToWorldPosition(mousePos);
            cursor.position = worldPosition;
        }
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
           
            float zDepth = cursor.position.z;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDepth));
            return worldPos;
        }

       
        return cursor.position;
    }
}