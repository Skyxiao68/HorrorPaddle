using UnityEngine;
using UnityEngine.UI;
public class menuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject scoreBoard;
    public GameObject pauseButton;


    private void Start()
    {
        pauseMenu.SetActive(false);
        scoreBoard.SetActive(true);
        pauseButton.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
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
