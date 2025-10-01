// Mouse Cursor to Move 3D object. 
//https://www.bilibili.com/video/BV1Jp4y1y71C/?spm_id_from=333.337.search-card.all.click&vd_source=735fcdc7dec66ac9bffaf8eac1a52072


using UnityEngine;
using UnityEngine.InputSystem;

public class menuNav : MonoBehaviour
{
    public PlayerInputController inputControl;
    public float mouseSens = 1f;
    public float gamepadSens = 10f;
    public Transform cursor;

    // ѡ��cursor�ƶ���ƽ��
    public enum MovementPlane { XZ, XY }
    public MovementPlane movementPlane = MovementPlane.XZ;

    public float cursorHeight = 0f;

    private Camera mainCamera;
    private Vector2 navigateInput;
    private Vector3 currentCursorPosition;
    private bool usingMouse = true;

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

        if (cursor == null)
        {
            Debug.LogError("Cursor transform is not assigned!");
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // ��ʼ�����λ��
        if (cursor != null)
        {
            currentCursorPosition = cursor.position;
        }
    }

    private void Update()
    {
        // ��ȡ�������
        Vector2 mousePos = inputControl.UI.Point.ReadValue<Vector2>();

        // ��ȡ�������루WASD/�ֱ���
        navigateInput = inputControl.UI.Navigate.ReadValue<Vector2>();

        // ������뷽ʽ�л�
        if (mousePos.sqrMagnitude > 0.01f)
        {
            usingMouse = true;
        }
        else if (navigateInput.sqrMagnitude > 0.01f)
        {
            usingMouse = false;
        }

        // �������뷽ʽ���¹��λ��
        if (usingMouse)
        {
            // ʹ��������
            currentCursorPosition = ScreenToWorldPosition(mousePos);
        }
        else
        {
            // ʹ�õ���������ƣ�WASD/�ֱ���
            if (movementPlane == MovementPlane.XZ)
            {
                // ��XZƽ�����ƶ�
                currentCursorPosition.x += navigateInput.x * gamepadSens * Time.deltaTime;
                currentCursorPosition.z += navigateInput.y * gamepadSens * Time.deltaTime;
            }
            else // XYƽ��
            {
                // ��XYƽ�����ƶ�
                currentCursorPosition.x += navigateInput.x * gamepadSens * Time.deltaTime;
                currentCursorPosition.y += navigateInput.y * gamepadSens * Time.deltaTime;
            }

            // ���ƹ������Ļ��Χ��
            ClampCursorToScreen();
        }

        // Ӧ�ù��λ��
        if (cursor != null)
        {
            cursor.position = currentCursorPosition;
        }
    }

    private Vector3 ScreenToWorldPosition(Vector2 screenPosition)
    {
        if (movementPlane == MovementPlane.XZ)
        {
            // ����XZƽ�棬����һ����XZƽ��ƽ�е����߼��
            Plane plane = new Plane(Vector3.up, new Vector3(0, cursorHeight, 0));
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
        }
        else // XYƽ��
        {
            // ����XYƽ�棬ʹ�ù̶���Z���
            float zDepth = cursor.position.z;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDepth));
            return worldPos;
        }

        // ������߼��ʧ�ܣ����ص�ǰλ��
        return currentCursorPosition;
    }

    private void ClampCursorToScreen()
    {
        // ����������ת��Ϊ��Ļ����
        Vector3 screenPos = mainCamera.WorldToScreenPoint(currentCursorPosition);

        // ��������Ļ��Χ��
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        // ת������������
        currentCursorPosition = ScreenToWorldPosition(new Vector2(screenPos.x, screenPos.y));
    }
}