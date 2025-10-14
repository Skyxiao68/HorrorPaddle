//Wrote this by ourself

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
public class IntermissionText : MonoBehaviour
{
    public PlayerInputController inputControl;
    public float inputFire;

    public GameObject collider;
   
    public bool hasBat =false;
    public TextMeshProUGUI moveMentText;
    public bool backtrack = false;
 
    public bool voiceOn = true ;

    private AudioSource voiceSource;
    public AudioClip letMeOut;
    public bool letMeOutB = true;

    public AudioClip greeting;
    public bool greetingB = true;

    public AudioClip pickMeUp;
    public AudioClip iAmPickedUp;
    public bool pickMeUpB = true;

    public AudioClip playWithBall;
    public bool playWithBallB = true;

    public AudioClip rules;
    public bool rulesB= true;

    public AudioClip haruUrara;
    public bool haruUraraB = true;

    public AudioClip batOldFriend;
    public void Awake()
    {
        inputControl = new PlayerInputController();
        voiceSource = GetComponent<AudioSource>();
       
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    public void OnTriggerStay(Collider collision)
    {
        if ( voiceOn == false)
        {
            return;
        }
   

        if (collision.CompareTag("Movement")&& letMeOutB == true)
        {
          //You beat Rudolph.... Honestly he was always a bit of a pushover.Now our next opponet is Alice and for her you have to learn how to jump, she will not be pushed over. 
            if (!voiceSource.isPlaying)
            voiceSource.PlayOneShot(letMeOut);
            letMeOutB =false;

        }
        if (collision.CompareTag("Welcome")&& greetingB == true)
        {
          // Stop... before you go on i just need to clear some stuff up with you, firstly this place cis built by someone with zero artistic skills, and yet he is so full of himself that he is actually proud of it, next do not try and solce the secret to this room. Seeing as what happend last time you will not go far and that room is terrifying, next thing read all these notes on the wall, i use a great deal of energy to make them visible if you ignore them i will kill you, next you can keep going.  
          if (!voiceSource.isPlaying)
          voiceSource.PlayOneShot (greeting);
            greetingB =false;

            if (greetingB == false)
            {
                StartCoroutine(WaitForVoice());
            }
        }
        if (collision.CompareTag("BatPickup")&& pickMeUpB == true && voiceOn)
        {
          if  (!voiceSource.isPlaying)
            { 

           //What has the show man done to them, these are all the past contestant bodies, i did not see this, Heyyy is that .... is that .... oh no .... my body , what has he done to my body. Now i am angry i am going to win and if you dare lose against Alice i am going to kill you myself. 
            voiceSource.PlayOneShot(pickMeUp);
            pickMeUpB =false;
          
            }
            

        }
        if (collision.CompareTag("PlayWithBall")&& playWithBallB == true )
        {
           //Did you not hear mw out lets just please move on that bastard needs to pay for what he did to my body.He mauled me so badly i do not even look like my world this guy has no artistic skill, definitley explains all these ugly handmade pieces of whatever they are. Could he not just stop at a furntirue store surely this guy is not being forced ....right?
            if (!voiceSource.isPlaying)
            {
                 voiceSource.PlayOneShot(playWithBall);
                playWithBallB =false;
            }
           

        }
        if (collision.CompareTag("Rules")&& rulesB == true)
        {
           // ok you dumb ass, listen up Alice is no pushover so i am borrowing you my powers, the wall skill, when summoned i will use some of my energy to push the ball and maintain a wall for a period of 5 seconds. Do not worry i am powerful enough to account for your skill issues
            if(!voiceSource.isPlaying)
            {
            voiceSource.PlayOneShot(rules);
            rulesB =false;
            }
           
        }
        if (collision.CompareTag("LastMin")&& haruUraraB == true)
        {
            //Next thing, is the power of these cute ears i discovered in our previous match, when i focus on moving them and yes all this flapping is uncomfotable i will be able to slow down time, so all i have to do in my infinite wisdom and power is to try and flap these ears and time will slow down. i do not know how this works but it is not my fault the showman is such a weird person
            if (!voiceSource.isPlaying)
            {
                voiceSource.PlayOneShot(haruUrara);
            haruUraraB =false;
            }
           
        }
        if (collision.CompareTag("NextLevel"))
        {
           SceneManager.LoadSceneAsync(2);
        }
        if (collision.CompareTag("Finish") && hasBat == true)
        {
         //This room is so terrifying why the hell did you open it up, noo come onnnnn.... you really are an idiot please stop, lets get out of here the shwoman might catch us. 
          if (!voiceSource.isPlaying)
            {
               voiceSource.PlayOneShot(batOldFriend);
            hasBat = false;
            }
          
        }
    }
    public IEnumerator WaitForVoice()
    {
        yield return new WaitForSeconds(7);

        collider.SetActive(false);
    }

  
}
