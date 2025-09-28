using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public PlayerInputController inputControl;
    public Material playMaterial;
    public float click;

    public void Awake()
    {
        inputControl = new PlayerInputController();
        playMaterial.SetColor("_Color", Color.white);
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
        click = inputControl.UI.Click.ReadValue < float > ();
        if (other.gameObject.CompareTag("Ball"))
        {
            playMaterial.SetColor("_Color",Color.red);
            if (click == 1)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playMaterial.SetColor("_Color",Color.white);
    }
}
