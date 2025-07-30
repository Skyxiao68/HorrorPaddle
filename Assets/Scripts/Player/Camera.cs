using UnityEngine;

public class Camera : MonoBehaviour
{
    public float mouseSens;
    
    private float mouseX, mouseY; 

    public Transform player;

    public float xRotation; 



    public void Start()
{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        


}
    private void Update()
    {

        mouseX = Input.GetAxis("MouseX") * Time.deltaTime * mouseSens;
        mouseY = Input.GetAxis("MouseY") * Time.deltaTime * mouseSens;
        player.Rotate(Vector3.up * mouseX);
         
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 
        
        
        transform.localRotation= Quaternion.Euler(xRotation,0 ,0);
       
    }


}

