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
    public float maxSwingDisZ;

    public bool batCanSwing;
    public float inputSwing;
    public ParticleSystem hitParticle;
    void Awake()
    {

        batCanSwing = true;
        batCurrentPos = transform.localPosition;
        inputControl = new PlayerInputController();
        ThrowableObject throwbool = GetComponent<ThrowableObject>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            SpawnBallEffects();
        }
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
        if (inputSwing == 1)//&& batCanSwing ) add this later because calling shit in other scriots is pissing me off 
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
        swingDis.x = Mathf.Clamp(swingDis.x, batCurrentPos.x - maxSwingDisX, batCurrentPos.x + maxSwingDisX);
        swingDis.y = Mathf.Clamp(swingDis.y, batCurrentPos.y - maxSwingDisY, batCurrentPos.y + maxSwingDisY);
        swingDis.z = Mathf.Clamp(swingDis.z, batCurrentPos.z - maxSwingDisZ, batCurrentPos.z + maxSwingDisZ);
        Vector3 finalSwing = swingDis;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, finalSwing, swingSpeed);
        batCanSwing = false;

       
    }
    void SwingBatIn()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, batCurrentPos, swingBackSpeed);
        batCanSwing = true;
    }
    void SpawnBallEffects()
    {
        Instantiate (hitParticle, transform.position, Quaternion.identity);
    }
}