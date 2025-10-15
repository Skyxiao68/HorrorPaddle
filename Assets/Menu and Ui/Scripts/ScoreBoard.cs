using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public GameObject[] playerNumbers;
    public GameObject[] opponentNumbers;
    private ScoreManager score;

    [Header("Voice Lines")]
    public AudioClip[] playerScoreVoicelines; 
    public AudioClip[] opponentScoreVoicelines; 

    private AudioSource audioSource;

    private void Awake()
    {
        score = FindAnyObjectByType<ScoreManager>();
        audioSource = GetComponent<AudioSource>(); 
    }

    public void ShowPlayerNumber(int digit)
    {
        foreach (GameObject num in playerNumbers)
        {
            num.SetActive(false);
        }

        int index = digit % 10;
        if (index >= 0 && index < playerNumbers.Length)
        {
            playerNumbers[index].SetActive(true);
        }

        
        PlayRandomPlayerVoiceline();
    }

    public void ShowEnemyNumber(int digit)
    {
        foreach (GameObject num in opponentNumbers)
        {
            num.SetActive(false);
        }

        int index = digit % 10;
        if (index >= 0 && index < opponentNumbers.Length)
        {
            opponentNumbers[index].SetActive(true);
        }

        PlayRandomOpponentVoiceline();
    }

    private void PlayRandomPlayerVoiceline()
    {
        if (playerScoreVoicelines.Length > 0)
        {
            int randomIndex = Random.Range(0, playerScoreVoicelines.Length);
            audioSource.PlayOneShot(playerScoreVoicelines[randomIndex]);
        }
    }

    private void PlayRandomOpponentVoiceline()
    {
        if (opponentScoreVoicelines.Length > 0)
        {
            int randomIndex = Random.Range(0, opponentScoreVoicelines.Length);
            audioSource.PlayOneShot(opponentScoreVoicelines[randomIndex]);
        }
    }
}



//opponent score bat (level 1)

//Focus you got this remember both of our lives are at stake.
//What are you doing can you even see the ball
//Place the ball in the center of your sight i will handle the swinging
//Wait for the ball to turn red , fun tips with meee
//Ok what the hell can you focus.
//SKILL ISSUE!!!!


//opponent score rudolph
//Are you even trying i have not even started trying yet 
//Looks like this one is worse than you were Rou
//Yes keep giving me openings like that and this will be a cakewalk
// SKILL ISSUE!!!!!


//Player score bat (level 1)
//keep doing that please
//Well done..... for a dumbass
//That one was all meeeeee
//Nice job, you are beating the old and weak
//Hey Rudolph how about you retire already you old hag


//Player score rudoplh
//You are actually serious
//What a shot
//Huh the next one will not past me
// I think its bullshit that only you get to serve but oh noo, Sherry makes these stupid rules
// If i still had my legs you would have been done for


//player score Alice 
//Ohhh myyyyy, look at this little tryhard.
//You still dont know who you are messing with. 
//Just you wait i am going to make your life a living hell rou


//plauyer score bat (level 2)
//Yesss keep abusing my poowers
//There is no way we can lose, i am going to show that bastard after this
// All meeee
// You may not be so bad good job, stupid.



//opponent score Alice
//Let the massacre begin
//If you are struggling with me, my beloved sherry is going to tear you apart
//Why are you so useless
//Even by abusing Rou's powers you are still losing
//Hahahah skill issue!!!!



//opponent score bat (level 2) 
//Can you press E or Q please use my powers what is wrong with you
//Come onnnnn focus up , even if you got 2 braincells
//You are not stealing my oppurtunity for revenge,
//I am going to make sure you suffer when they turn you
// Once again you are having a skill issue.