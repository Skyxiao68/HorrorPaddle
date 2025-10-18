//SCORE & UI - How to make a Video Game in Unity (E07)
//Brackeys
// 25 July 2025
// Version 2
//https://youtu.be/TAGZxRMloyU?si=kx5MgXld_n3wJiKO


using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Animator ded;
    public GameObject winScreen;
    public GameObject loseScreen;
    public Rigidbody player;
    public ScoreBoard scoreBoard;
    public AudioClip loseSound;
    public AudioClip wonSound;
    private AudioSource wonSource;
    private AudioSource loseSource;

    public int pScore = 0;
    public int oScore = 0;
    public Transform targetsPosition;

    void Awake()
    {


    }
    void Start()
    {

        // loseScreen.SetActive(false);
        wonSource = GetComponent<AudioSource>();
        loseSource = GetComponent<AudioSource>();
        wonSource.enabled = true;

    }
    private void Update()
    {
        if (pScore >= 5)
        {
            PlayerWon();
            gameObject.SetActive(true);
            wonSource.PlayOneShot(wonSound);
        }
    }
    public void AddScoreP()
    {
        pScore++;

        if (scoreBoard != null)
        {
            scoreBoard.ShowPlayerNumber(pScore);
        }
        if (pScore >= 5)
        {
            PlayerWon();
            gameObject.SetActive(true);
            wonSource.PlayOneShot(wonSound);
        }
    }

    public void AddScoreO()
    {
        oScore++;

        if (scoreBoard != null)
        {
            scoreBoard.ShowEnemyNumber(oScore);
        }

        if (oScore >= 5)
            PlayerLost();
    }



    public void PlayerWon()
    {
        Vector3 startPosition = winScreen.transform.position;
        Vector3 targetPosition = targetsPosition.position;

        EnablePlayerControls(false);
        winScreen.transform.position = Vector3.MoveTowards(startPosition, targetPosition, 10f * Time.deltaTime);

    }
    public IEnumerator MoveUp()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetsPosition.position;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            yield return null;

            elapsedTime += Time.deltaTime;
        }
        transform.position = targetPosition;
    }
    public void PlayerLost()
    {


        player.AddTorque(Random.onUnitSphere * 500000000000f, ForceMode.Impulse);

        ded.SetBool("Died", true);

        loseScreen.SetActive(true);
        EnablePlayerControls(false);

        Invoke(nameof(ShowLoseScreen), 2f);



    }
    private void ShowLoseScreen()
    {
        ded.SetBool("Died", false);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        loseSource.PlayOneShot(loseSound);
        

    }


    void EnablePlayerControls(bool enable)
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {


                {
                    script.enabled = enable;
                }
            }
        }
    }


}