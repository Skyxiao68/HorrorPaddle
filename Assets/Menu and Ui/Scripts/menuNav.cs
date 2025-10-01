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

    // 选择cursor移动的平面
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

        // 初始化光标位置
        if (cursor != null)
        {
            currentCursorPosition = cursor.position;
        }
    }

    private void Update()
    {
        // 获取鼠标输入
        Vector2 mousePos = inputControl.UI.Point.ReadValue<Vector2>();

        // 获取导航输入（WASD/手柄）
        navigateInput = inputControl.UI.Navigate.ReadValue<Vector2>();

        // 检测输入方式切换
        if (mousePos.sqrMagnitude > 0.01f)
        {
            usingMouse = true;
        }
        else if (navigateInput.sqrMagnitude > 0.01f)
        {
            usingMouse = false;
        }

        // 根据输入方式更新光标位置
        if (usingMouse)
        {
            // 使用鼠标控制
            currentCursorPosition = ScreenToWorldPosition(mousePos);
        }
        else
        {
            // 使用导航输入控制（WASD/手柄）
            if (movementPlane == MovementPlane.XZ)
            {
                // 在XZ平面上移动
                currentCursorPosition.x += navigateInput.x * gamepadSens * Time.deltaTime;
                currentCursorPosition.z += navigateInput.y * gamepadSens * Time.deltaTime;
            }
            else // XY平面
            {
                // 在XY平面上移动
                currentCursorPosition.x += navigateInput.x * gamepadSens * Time.deltaTime;
                currentCursorPosition.y += navigateInput.y * gamepadSens * Time.deltaTime;
            }

            // 限制光标在屏幕范围内
            ClampCursorToScreen();
        }

        // 应用光标位置
        if (cursor != null)
        {
            cursor.position = currentCursorPosition;
        }
    }

    private Vector3 ScreenToWorldPosition(Vector2 screenPosition)
    {
        if (movementPlane == MovementPlane.XZ)
        {
            // 对于XZ平面，创建一个与XZ平面平行的射线检测
            Plane plane = new Plane(Vector3.up, new Vector3(0, cursorHeight, 0));
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (plane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
        }
        else // XY平面
        {
            // 对于XY平面，使用固定的Z深度
            float zDepth = cursor.position.z;
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, zDepth));
            return worldPos;
        }

        // 如果射线检测失败，返回当前位置
        return currentCursorPosition;
    }

    private void ClampCursorToScreen()
    {
        // 将世界坐标转换为屏幕坐标
        Vector3 screenPos = mainCamera.WorldToScreenPoint(currentCursorPosition);

        // 限制在屏幕范围内
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        // 转换回世界坐标
        currentCursorPosition = ScreenToWorldPosition(new Vector2(screenPos.x, screenPos.y));
    }
}