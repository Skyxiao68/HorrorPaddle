//We wrote this ourself using the Unity Documentation on Ui for refreshers



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menuController : MonoBehaviour
{
    public PlayerInputController inputControl;

    public GameObject pauseMenu;
    public GameObject scoreBoard;
    public GameObject pauseButton;
    public GameObject settingsMenu;
    public GameObject paddleBat;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject storyScene;

   public float pause;
    private CharacterController CC;
    private void Awake()
    {
        Time.timeScale = 1f;
        
        pauseMenu.SetActive(false);
        scoreBoard.SetActive(true);
        pauseButton.SetActive(true);
        settingsMenu.SetActive(false);
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
            paddleBat.SetActive(false);
            Time.timeScale = 0f;
        
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        pauseMenu.SetActive(false);
        scoreBoard.SetActive(true);
        pauseButton.SetActive(true);
        paddleBat.SetActive(true) ;
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    
    public void Settings()
    {
        scoreBoard.SetActive(false);
        pauseMenu.SetActive(false );
        settingsMenu.SetActive(true);
    }
   public void SettingsBack()
    {
        pauseMenu.SetActive(true );
        settingsMenu.SetActive(false);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void EndStory()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        storyScene.SetActive(false);
    }
   
}
