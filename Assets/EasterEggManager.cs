using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EasterEggManager : MonoBehaviour
{
    public GameObject[] easterEggOrder;
    public AudioClip success;
    private AudioSource audioSource;
    private List<GameObject> playerSequence = new List<GameObject>();

    public float sequenceTimeout = 5f;
    private float timeSinceLastHit;

    public GameObject theDoor;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        timeSinceLastHit += Time.deltaTime;
        if (timeSinceLastHit >= sequenceTimeout && playerSequence.Count > 0)
        {
            Debug.Log("The sequence stuff reset and stiff");
            ResetPuzzle();
        }
    }

   public bool RegisterBall(GameObject target)
    {
        timeSinceLastHit = 0f;
        playerSequence.Add(target);

        for (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != easterEggOrder[i])
            {

                Debug.Log("Wrong order reset ");
                ResetPuzzle();
                return false;
            }


        }
        Debug.Log("Corrext Sequence" + playerSequence.Count + "/" + playerSequence.Count + "/" + easterEggOrder.Length);

        if (playerSequence.Count == easterEggOrder.Length)
        {
            Debug.Log("Unlocked");
            audioSource.PlayOneShot(success);
            UnlockSecret();
            ResetPuzzle();
        }
        return true;
    }
    private void ResetPuzzle()
    {
        playerSequence.Clear();
        timeSinceLastHit = 0f;
    }

    private void UnlockSecret()
    {
        theDoor.SetActive(false);

    }
}
