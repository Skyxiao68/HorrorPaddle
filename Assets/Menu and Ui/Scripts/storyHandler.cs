using UnityEngine;

public class storyHandler : MonoBehaviour
{

    public PlayerInputController inputControl;
    public GameObject storyStuff;
    public float inputClick;

    private void Awake()
    {
        inputControl = new PlayerInputController();
        storyStuff.SetActive(true);
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
        inputClick = inputControl.UI.MenuClick.ReadValue<float>();

        if (inputClick == 1)
        { 
         storyStuff.SetActive(false);
        }
    }


}
