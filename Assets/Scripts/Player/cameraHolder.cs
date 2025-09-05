using UnityEngine;

public class cameraHolder : MonoBehaviour
{
    public Transform camHolder;

    public void Update()
    {
        transform.position = camHolder.position;
    }
}
