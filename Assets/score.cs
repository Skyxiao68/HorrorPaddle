using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI opponentScore;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Rigidbody player;
   
    public int pScore = 0;
    public int oScore = 0;

    void Start()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        UpdateScoreUI();
    }

    public void AddScoreP()
    {
        pScore++;
        UpdateScoreUI();
     

        if (pScore >= 5) 
            PlayerWon();
    }

    public void AddScoreO()
    {
        oScore++;
        UpdateScoreUI();
       

        if (oScore >= 5) 
            
          PlayerLost();
    }

    private void UpdateScoreUI()
    {
        playerScore.text = "Your Score: " + pScore;
        opponentScore.text = "Opponent Score: " + oScore;
    }

    public void PlayerWon()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void PlayerLost()
    {
        
      player.AddTorque(Random.onUnitSphere * 500000000000f, ForceMode.Impulse);



        Invoke(nameof(ShowLoseScreen), 3f);


   
    }
    private void ShowLoseScreen()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}