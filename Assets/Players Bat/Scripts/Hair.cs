using UnityEngine;

public class Hair : MonoBehaviour
{
    public GameObject face;
    void Update()
    {
        transform.position = face.transform.position;
        transform.rotation = face.transform.rotation;
        transform.localScale = face.transform.localScale;
    }
}
