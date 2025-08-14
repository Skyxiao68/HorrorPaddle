using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI moveMentText;
   
    public GameObject tutTextObj;


   
    public void OnTriggerEnter(Collider collision)
    {
        tutTextObj.SetActive(true);

        if (collision.CompareTag("Movement"))
        {
            moveMentText.text = "FFS WORK BITCH ";
        }
        if (collision.CompareTag("Welcome"))
        {
            moveMentText.text = "Fuck";
        }
    }

    public void OnTriggerExit(Collider other)
    {
        tutTextObj.SetActive(false);
    }
}
