using UnityEngine;
using UnityEngine.UI;
public class menuController : MonoBehaviour
{
    public PlayerInputController inputControl;

    public GameObject pauseMenu;
    public GameObject scoreBoard;
    public GameObject pauseButton;

   public float pause;
    private CharacterController CC;
    private void Awake()
    {
      
        pauseMenu.SetActive(false);
        scoreBoard.SetActive(true);
        pauseButton.SetActive(true);
        inputControl = new PlayerInputController();
        CC = gameObject.GetComponent<CharacterController>();
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

        pause = inputControl.UI.Pause.ReadValue<float>();

        if (pause == 1) { Pause(); }
    }

    public void Pause()
    {
       
        
            Cursor.lockState = CursorLockMode.Confined;
             Cursor.visible = true;
            Debug.Log("I am pausing");
            pauseMenu.SetActive(true);
            scoreBoard.SetActive(false);
            pauseButton.SetActive(false);

            Time.timeScale = 0f;
        
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        pauseMenu.SetActive(false);
        scoreBoard.SetActive(true);
        pauseButton.SetActive(true);

        Time.timeScale = 1f;
    }
}
