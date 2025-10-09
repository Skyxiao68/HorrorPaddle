using UnityEngine;
using UnityEngine.UI;

public class MenuDebugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== MENU DEBUGGER ===");
        Debug.Log("Scene: " + gameObject.scene.name);

        // Check basic components
        CheckCamera();
        CheckEventSystem();
        CheckCanvas();
    }

    void CheckCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) Debug.LogError(" No main camera found!");
        else Debug.Log("? Main camera: " + cam.name);
    }

    void CheckEventSystem()
    {
        var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem == null) Debug.LogError("? No EventSystem found!");
        else Debug.Log("? EventSystem: " + eventSystem.name);
    }

    void CheckCanvas()
    {
        var canvas = GetComponent<Canvas>();
        if (canvas == null) Debug.LogError("? No Canvas component found!");
        else Debug.Log("? Canvas found - Render Mode: " + canvas.renderMode);
    }
}