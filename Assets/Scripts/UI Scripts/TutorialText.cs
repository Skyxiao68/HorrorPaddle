//Wrote this by ourself

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TutorialText : MonoBehaviour
{
    public PlayerInputController inputControl;
    public float inputFire;
    public GameObject bat;
    public GameObject propBat;
    public bool hasBat =false;
    public TextMeshProUGUI moveMentText;
    public bool backtrack = false;
    public GameObject tutTextObj;
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
        bat.SetActive(false);
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    public void OnTriggerEnter(Collider collision)
    {
        if ( voiceOn == false)
        {
            return;
        }
        tutTextObj.SetActive(true);

        if (collision.CompareTag("Movement")&& letMeOutB == true)
        {
            // Let me out of here you bastards! This is not fair ! The Showman cheated, he knew i was too good for him. 

            voiceSource.PlayOneShot(letMeOut);
            letMeOutB =false;

        }
        if (collision.CompareTag("Welcome")&& greetingB == true)
        {
          // Wow ! Hello there, you are one ugly piece of shit, you look dumb too, what kind of rat hole did you crawl out of. I supposeyou will have to do please come and pick me up i need to get my revenge.
          voiceSource.PlayOneShot (greeting);
            greetingB =false;
        }
        if (collision.CompareTag("BatPickup")&& pickMeUpB == true)
        {
            // Alright you took your time coming here , come on now pick me up , come on press buttons or something pick me up, you seriously dont know what to do? Pick me up , come on , you can do it just spam every button in front of you come one pick me. What is taking you so long please pick me up, You seriously are not going to pick me up , Ok ok the button is the shooting one pick me up now.

            voiceSource.PlayOneShot(pickMeUp);
            inputFire = inputControl.Gameplay.Fire.ReadValue<float>();
            if (inputFire == 1)
            {
                bat.SetActive(true);
                propBat.SetActive(false);
                voiceSource.PlayOneShot(iAmPickedUp);
                // Well done! You can follow basic instructions , if it takes you this long to complete a simple task i'd hate to see which world you were dragged in from.
             pickMeUpB =false;
                hasBat = true;
            }
          

        }
        if (collision.CompareTag("PlayWithBall")&& playWithBallB == true )
        {
           // Sure you would love to play with some balls dont you. Your arms look so small it looks like you cant even hold me upright, we are soo losing against Rudolph i am going to become another trophy, why me , he cheated i deserve a second chance , dont look at me i bet you are too dumb to even solve the secret to this place we all have, you can be the first to not. Go on prove me right. 

            voiceSource.PlayOneShot(playWithBall);
            playWithBallB =false;

        }
        if (collision.CompareTag("Rules")&& rulesB == true)
        {
            //AAAAAAAhhhhhhh shit here we go again, i think i am going to be sick with all the swinging i am about to do because you do not looked too skilled to me;
            voiceSource.PlayOneShot(rules);
            rulesB =false;
        }
        if (collision.CompareTag("LastMin")&& haruUraraB == true)
        {
            // You figured it out , so what it was obvious i would be surpised if you did not, anyway the person who runs this place is a bit crazy obsessed with horses and cats and my people , His a real weirdo , where i come from we call these people weebs, he seems to think that this haru urara person is his daughter or something , these cat , donkey, horse ears or whatever is on me is really anoying, the creator of this place clearly lacks artistic skill, what a dumbass i should give him lessons. 
            voiceSource.PlayOneShot(haruUrara);
            haruUraraB =false;
        }
        if (collision.CompareTag("NextLevel"))
        {
           SceneManager.LoadSceneAsync(2);
        }
        if (collision.CompareTag("Removed") && hasBat == true)
        {
           // I am sorry girl , i did what i could , i promise  will make the showman pay for your family , Hey you quit staring or we will end up here when you lose this. 
           voiceSource.PlayOneShot(batOldFriend);
            hasBat = false;
        }
    }

  
}
