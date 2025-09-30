using UnityEngine;

public class Quit : MonoBehaviour
{
    public PlayerInputController inputControl;
    public Material quitMaterial;
    public float click;
    void Awake()
    {
        quitMaterial.SetColor("_Color", Color.white);
        inputControl = new PlayerInputController();
    }


    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        click = inputControl.UI.Click.ReadValue<float>();
        if (other.gameObject.CompareTag("Ball"))
        {
            quitMaterial.SetColor("_Color", Color.red);
            if (click == 1 )
            {
                Application.Quit();
                Debug.Log("RageQuit");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        quitMaterial.SetColor("_Color", Color.white);
    }
}
