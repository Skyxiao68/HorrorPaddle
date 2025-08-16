//Wrote this by ourself

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialText : MonoBehaviour
{
    public TextMeshProUGUI moveMentText;
   
    public GameObject tutTextObj;


   
    public void OnTriggerEnter(Collider collision)
    {
        tutTextObj.SetActive(true);

        if (collision.CompareTag("Movement"))
        {
            moveMentText.text = "WASD to move and use your mouse to look around.Left stick to move and Right stick to look around(please stop walking to read these) When you pause please navigate the menues with your mouse";
        }
        if (collision.CompareTag("Welcome"))
        {
            moveMentText.text = "Welcome to Horror paddle prototype 1";
        }
        if (collision.CompareTag("BatPickup"))
        {
            moveMentText.text = "On your right is a bat go and pick it up! Use Left click/ Right trigger on controller to swing try it out";
        }
        if (collision.CompareTag("PlayWithBall"))
        {
            moveMentText.text = "In the next room is a ball, you are free to play around with it until you are comfortable, once you are done proceed to the door on your right, Left Shift/L3 on controller can be used to sprint";
        }
        if (collision.CompareTag("Rules"))
        {
            moveMentText.text = "The rules for now are simple, try to hit the ball into the quadrants marked as red whilst keeping it out of your blue quadrants, the first to score 5 points wins ";
        }
        if (collision.CompareTag("LastMin"))
        {
            moveMentText.text = "Use your positioning and mouse aim to control the ball your arc is predetermined to make it easier, and do not hold in the hit button your shot will be weaker wait for the hit window, the window is not present in this tutorial to get you used to hitting the ball, proceed to the door on your right when you ready";
        }
        if (collision.CompareTag("NextLevel"))
        {
            moveMentText.text = "Good Luck!";
            SceneManager.LoadSceneAsync(2);
        }
        if (collision.CompareTag("Removed"))
        {
            moveMentText.text = ".";
        }
    }

  
}
