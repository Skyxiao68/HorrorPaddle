using UnityEngine;
using System;
public class HeadBob : MonoBehaviour
{
    [Header("Head bob controls")]

    [SerializeField]private bool activateFeature = true;

    [SerializeField, Range(0, 50f)] private float amplitude = 1.0f;
    [SerializeField, Range(0, 100f)] private float frequency = 10.0f;

    [SerializeField] private Transform camera;
    [SerializeField] private Transform camHolder;

    private float toggleSpeed = 4f;
    private Vector3 startPos;
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        startPos = camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!activateFeature) return;
       
       CheckMotion();
        ResetPosition();
        camera.LookAt(FocusTarget());

        //Aded a running fearture 
    }
    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion * Time.deltaTime;
    }
    private void CheckMotion()
    {
        float speed = playerController.CurrentMovementSpeed;

        if (speed < toggleSpeed) 
            
        return; 
      

        PlayMotion(FootStepMotion());
    }
    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency/2) * amplitude * 2;
        return pos;
    }
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + camHolder.localPosition.y, transform.position.z);
        pos += camHolder.forward * 15.0f;
        return pos;
    }
}
