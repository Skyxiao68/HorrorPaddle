using UnityEngine;

public class ScoreBoard : MonoBehaviour
{

    public GameObject[] playerNumbers;
    public GameObject[] opponentNumbers;
    private ScoreManager score;

    private void Awake()
    {
       score = FindAnyObjectByType<ScoreManager>();
    }
    public void ShowPlayerNumber(int digit)
    {
        foreach(GameObject num in playerNumbers)
        {
            num.SetActive(false);
        }

        int index = digit % 10;
        if (index >= 0 && index < playerNumbers.Length)
        { 
            playerNumbers[index].SetActive(true);   
        }
    }

    public void ShowEnemyNumber(int digit ) {
        foreach (GameObject num in opponentNumbers)
        {
            num.SetActive(false);
        }

        int index = digit % 10;
        if (index >= 0 && index < opponentNumbers.Length)
        {
            opponentNumbers[index].SetActive(true);
        }
    }
}
