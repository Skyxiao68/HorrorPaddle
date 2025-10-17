//We wrote this ourself using the Unity Documentation on Ui for refreshers



using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menuController : MonoBehaviour
{
    public UIFishMovement fishMovement;
    public PlayerInputController inputControl;

    public GameObject pauseMenu;
  
    public GameObject pauseButton;
    public GameObject settingsMenu;
    public GameObject paddleBat;

    public GameObject storyScene;

    private bool paused;
    private bool pauseInputProcessed = false;
    public float pause;
    private CharacterController CC;
    private void Awake()
    {
        Time.timeScale = 1f;
        
        pauseMenu.SetActive(false);
      
        pauseButton.SetActive(true);
        settingsMenu.SetActive(false);
        inputControl = new PlayerInputController();
        CC = gameObject.GetComponent<CharacterController>();
        UIFishMovement fishMovement = FindAnyObjectByType<UIFishMovement>();
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
        float pause = inputControl.UI.Pause.ReadValue<float>();

        if (pause == 1 && !pauseInputProcessed)
        {
            paused = !paused;

            if (paused)
                Pause();
            else
                Resume();

            pauseInputProcessed = true;
        }
        else if (pause == 0)
        {
            pauseInputProcessed = false;
        }
    }

    public void Pause()
    {
       
        
            Cursor.lockState = CursorLockMode.Confined;
             Cursor.visible = true;
            Debug.Log("I am pausing");
            pauseMenu.SetActive(true);
           
            pauseButton.SetActive(false);
            paddleBat.SetActive(false);
            Time.timeScale = 0f;
        
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        pauseMenu.SetActive(false);
       
        pauseButton.SetActive(true);
        paddleBat.SetActive(true) ;
        Time.timeScale = 1f;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);  
        Time.timeScale = 1f;
    }
    
    public void Settings()
    {
       
       
     
       
        pauseMenu.SetActive(false );
        settingsMenu.SetActive(true);
    }
   public void SettingsBack()
    {
        pauseMenu.SetActive(true );
        settingsMenu.SetActive(false); 
      //  fishMovement.SetMovementActive(false); 
    }
    public void Restart()
    {
        Destroy(FindObjectOfType<UnityEngine.EventSystems.EventSystem>().gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void EndStory()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        storyScene.SetActive(false);
    }
   
}
