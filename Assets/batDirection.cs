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
    public float ishitting;

    private bool leftDirectionPressed;
    private bool rightDirectionPressed;

    private enum batState { Forward, Right, Left }

    [SerializeField] private batState currentState = batState.Forward;

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

    }
    void InputHandler()
    {
        if (directionSwitchRight == 1)
        {
            currentState = currentState switch
            {
                batState.Forward => batState.Right,
                batState.Right => batState.Left,
                batState.Left => batState.Forward,


            };

        }


        else if (directionSwitchLeft == 1)
        {
            currentState = currentState switch
            {
                batState.Forward => batState.Left,
                batState.Left => batState.Right,
                batState.Right => batState.Forward,
            };
        }

        
    }
    void StateHandler()
    {
        switch (currentState)
        {
            case batState.Forward:
                bat.SetColor("_BaseColor", Color.green);
                if (ishitting == 1) 
                {
                    animator.SetTrigger("Forward");
                }
                animator.SetTrigger("Idle");
                break;

                case batState.Right:
                bat.SetColor("_BaseColor", Color.blue);
                if (ishitting == 1)
                {
                    animator.SetTrigger("Right 1");
                }
                animator.SetTrigger("Idle");
                break;
                case batState.Left:
                bat.SetColor("_BaseColor", Color.magenta);
                if (ishitting == 1)
                {
                    animator.SetTrigger("Left 1");
                }
                animator.SetTrigger("Idle");
                break;


        }
    }
}








       

