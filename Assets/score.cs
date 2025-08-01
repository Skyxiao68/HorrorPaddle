using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class score : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI opponentScore;

    private int pScore = 0;
    private int oScore = 0;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerWall"))
        {
        }
        if (collision.CompareTag("OpponentWall"))
        {
            
        }
    }
    void Start ()
    {

         playerScore.text = "Your Score:   " + pScore.ToString();

        opponentScore.text = "Opponent Score:   " + oScore.ToString();
    }

    public void AddScoreP()
    {
        pScore += 1;
        playerScore.text = "Your Score:   " + pScore.ToString();
    }

    public void AddScoreO() 
    {
        oScore += 1;
        opponentScore.text = "Opponent Score:   " + oScore.ToString();
    }
}
