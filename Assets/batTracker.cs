using System.Collections;
using UnityEngine;

public class batTracker : MonoBehaviour
{
    public PlayerInputController inputControl;

    public float swingDistance;
    public float swingSpeed;
    public float swingBackSpeed;
    private Vector3 batCurrentPos;

    public float maxSwingDisX;
    public float maxSwingDisY;
  
    public bool batCanSwing;
    public float inputSwing ;

    void Awake()
    {

        batCanSwing = true;
        batCurrentPos = transform.localPosition;
        inputControl = new PlayerInputController();
        ThrowableObject throwbool = GetComponent<ThrowableObject>();
 
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }
    void Update()
    {
        
        inputSwing = inputControl.Gameplay.Fire.ReadValue<float>();
        if (inputSwing == 1 )//&& batCanSwing ) add this later because calling shit in other scriots is pissing me off 
        {
            SwingBatOut();
        }
        if (inputSwing == 0 && !batCanSwing) //Thjat bool is the key but again callijng shit in other shit is pissing me off
        {
            SwingBatIn();
        }

       
    }
    void SwingBatOut()
    {
       
      
        Vector3 swingDis = transform.localPosition + transform.forward * swingDistance;
        swingDis.x = Mathf.Clamp(swingDis.x,batCurrentPos.x - maxSwingDisX, batCurrentPos.x + maxSwingDisX);
        swingDis.y = Mathf.Clamp (-swingDis.y,batCurrentPos.y - maxSwingDisY, batCurrentPos.y + maxSwingDisY);
        Vector3 finalSwing = swingDis;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, finalSwing, swingSpeed);
        batCanSwing=false;
    }
    void SwingBatIn()
    {
        transform.localPosition = Vector3.MoveTowards (transform.localPosition, batCurrentPos, swingBackSpeed);
        batCanSwing=true;
    }

    IEnumerator SwingBatAnimation()
    {

            float swingSpeed = 10f;
            Vector3 swingArc = batCurrentPos + transform.forward * swingDistance;

            // Swing forward
            while (transform.localPosition != swingArc)
            {
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    swingArc,
                    swingSpeed * Time.deltaTime
                );
                yield return null;
            }

            // Pause (optional)
            yield return new WaitForSeconds(0.1f);

            // Return to original position
            while (transform.localPosition != batCurrentPos)
            {
                transform.localPosition = Vector3.MoveTowards(
                    transform.localPosition,
                    batCurrentPos,
                    swingSpeed * Time.deltaTime
                );
                yield return null;
            }
        }
    }





























    //bool isSwinging = false;
   // Vector3 defaultLocalPos; // Store starting position

   // void Start()
   // {
    //    defaultLocalPos = transform.localPosition;
   // }

  //  void Update()
  //  {
    //    if (ShouldHitBall() && !isSwinging)
     //   {
     //       StartCoroutine(SwingPaddle());
      //  }
 //   }

    //IEnumerator SwingPaddle()
   // {
   //     isSwinging = true;
   //
        // 1. Swing forward
      //  float swingSpeed = 10f;
        //Vector3 hitPos = CalculateHitPosition(); // Forward position
       // while (transform.localPosition != hitPos)
        //{
   //         transform.localPosition = Vector3.MoveTowards(
      //          transform.localPosition,
        //        hitPos,
          //      swingSpeed * Time.deltaTime
            //);
        //    yield return null;
        //}

        // 2. Brief pause (optional)
//        yield return new WaitForSeconds(0.1f);

        // 3. Retract
  //      while (transform.localPosition != defaultLocalPos)
    //    {
      //      transform.localPosition = Vector3.MoveTowards(
        //        transform.localPosition,
          //      defaultLocalPos,
            //    swingSpeed * Time.deltaTime
           // );
            //yield return null;
  //      }

    //    isSwinging = false;
  //  }
//}
