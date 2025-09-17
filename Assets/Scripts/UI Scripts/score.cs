//SCORE & UI - How to make a Video Game in Unity (E07)
//Brackeys
// 25 July 2025
// Version 2
//https://youtu.be/TAGZxRMloyU?si=kx5MgXld_n3wJiKO


using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI opponentScore;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Rigidbody player;
    public ScoreBoard scoreBoard;
   
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
        if (scoreBoard != null)
        { 
        scoreBoard.ShowPlayerNumber(pScore);
        }
        if (pScore >= 5) 
            PlayerWon();
    }

    public void AddScoreO()
    {
        oScore++;
        UpdateScoreUI();
        if (scoreBoard != null)
        {
         scoreBoard.ShowEnemyNumber(oScore);
        }

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