using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class batDirection : MonoBehaviour
{

    public PlayerInputController directionControl;
    public Material bat;

    public Animator animator;
    public int direction;
    public float directionSwitchLeft;
    public float directionSwitchRight;
    public float directionSwitchForward;
    public float ishitting;

    private bool leftDirectionPressed;
    private bool rightDirectionPressed;
    private bool forwardDirectionPressed;
    private bool canHit;
    public enum batState { Forward, Right, Left }

   public batState currentState = batState.Forward;

    void Awake()
    {
        animator = GetComponent<Animator>();

        directionControl = new PlayerInputController();
        bat.SetColor("_BaseColor", Color.green);
    }
    void OnEnable()
    {
        directionControl.Enable();

    }

    void OnDisable()
    {
        directionControl.Disable();

    }
    void Update()
    {


        InputHandler();
        StateHandler();


        ishitting = directionControl.Gameplay.TriggerAnimation.ReadValue<float>();

        directionSwitchLeft = directionControl.Gameplay.ChangeLeft.ReadValue<float>();
        directionSwitchRight = directionControl.Gameplay.ChangeRight.ReadValue<float>();
        directionSwitchForward =directionControl.Gameplay.ChangeForward.ReadValue<float>();

    }
    void InputHandler()
    {
        if (directionSwitchRight == 1 && rightDirectionPressed==false)
        {
            rightDirectionPressed = true;
            currentState = batState.Right;
        }
        if (directionSwitchRight == 0)
            {
                rightDirectionPressed = false;
            }

        if (directionSwitchLeft == 1 && leftDirectionPressed == false)
        {
            leftDirectionPressed = true;
            currentState = batState.Left;
           
        }
        if (directionSwitchLeft == 0)
        {
                leftDirectionPressed = false;
        }


        if (directionSwitchForward == 1 && forwardDirectionPressed == false)
        {
            forwardDirectionPressed = true;
            currentState = batState.Forward;
        }
        if (directionSwitchForward == 0)
        {
            forwardDirectionPressed = false;
        }
        
    }
    void StateHandler()
    {
        switch (currentState)
        {
            case batState.Forward:
                bat.SetColor("_BaseColor", Color.green);
                if (ishitting == 1 && canHit==true) 
                {
                    animator.SetTrigger("Forward");
                    canHit = false;
                }
              
                break;

                case batState.Right:
                bat.SetColor("_BaseColor", Color.blue);
                if (ishitting == 1 && canHit == true)
                {
                    animator.SetTrigger("Right 1");
                    canHit=false;
                }
               
                break;
                case batState.Left:
                bat.SetColor("_BaseColor", Color.magenta);
                if (ishitting == 1 && canHit == true)
                {
                    animator.SetTrigger("Left 1");
                    canHit=false;
                }
               
                break;


        }
        if (ishitting == 0)
        {
            animator.SetTrigger("Idle");
            canHit = true;
        }
    }
}








       

