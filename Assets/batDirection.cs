using UnityEngine;
using UnityEngine.InputSystem;

public class batDirection : MonoBehaviour
{

    public PlayerInputController directionControl;
    public Material bat;


    public int direction;
    public float directionSwitchLeft;
    public float directionSwitchRight;
 
    void Awake()
    {
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


        directionSwitchLeft =directionControl.Gameplay.ChangeLeft.ReadValue<float>();
      
        directionSwitchRight = directionControl.Gameplay.ChangeRight.ReadValue<float>();
      
        
        if (directionSwitchLeft > 0.1f) Debug.Log("switchleft pressed");
        if (directionSwitchRight > 0.1f) Debug.Log("switchright pressed");

        if (directionSwitchLeft == 1)
        {
            switch (direction)
            {
                case 1:
                    Forward();
                    break;
                case 2:
                    Left();
                    break;
                case 3:
                    Right();
                    break;
            }

            direction = (direction == 1) ? 2 : 1;

            
        }
        if (directionSwitchRight == 1)
        {
            switch (direction)
            {
                case 1:
                    Forward();
                    break;
                case 2:
                    Right();
                    break;
                case 3:
                    Left();
                    break;

            }
            direction = (direction == 1) ? 2 : 1;


        }
        void Forward()
        {
            bat.SetColor("_BaseColor", Color.green);
        }
        void Left()
        {
            bat.SetColor("_BaseColor", Color.magenta);
        }
        void Right()
        {
            bat.SetColor("_BaseColor", Color.blue);
        }
    }
}
