using UnityEngine;

public class Camera : MonoBehaviour
{
    public float xSens;
    public float ySens;

    public Transform playerBody;

    public float xRotation;
    public float yRotation;



    public void Start()
{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        


}
    private void Update()
    {
        float mouseX = Input.GetAxisRaw ("MouseX") *Time.deltaTime * xSens;

        float mouseY = Input.GetAxisRaw("MouseY") * Time.deltaTime * ySens;


        yRotation += mouseX;
        
        xRotation -= mouseY;

        Mathf.Clamp (yRotation, -90, 90);

        Mathf.Clamp(xRotation, -90, 90);

        transform.rotation= Quaternion.Euler(xRotation, yRotation, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }


}

