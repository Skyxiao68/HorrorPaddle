using UnityEngine;

public class storyHandler : MonoBehaviour
{

    public PlayerInputController inputControl;
    public GameObject storyStuff;
    public float inputFire;

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
        inputFire = inputControl.Gameplay.Fire.ReadValue<float>();

        if (inputFire == 1)
        { 
         storyStuff.SetActive(false);
        }
    }


}
