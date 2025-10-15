using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class MasterEye : MonoBehaviour
{
    public GameObject[] topEye;
    public GameObject[] bottomEye;

    public float speed = 2.0f;

    [Header("Eye Closing Settings")]
    public float closedZOffset = -0.1f; 
    public float bottomClosedZOffset = 0.1f; 

    private Vector3[] topeyeOrigin;
    private Vector3[] botEyeOrigin;
    private bool isClosing = false;
    private bool isOpening = false;

    private void Start()
    {
        topeyeOrigin = new Vector3[topEye.Length];
        botEyeOrigin = new Vector3[bottomEye.Length];
        for (int i = 0; i < topEye.Length; i++)
        {
            if (topEye[i] != null)
                topeyeOrigin[i] = topEye[i].transform.position;
        }

        for (int i = 0; i < bottomEye.Length; i++)
        {
            if (bottomEye[i] != null)
                botEyeOrigin[i] = bottomEye[i].transform.position;
        }
    }

    private void Update()
    {
        if (isClosing)
        {
            CloseEyesSmoothly();
        }
        else if (isOpening)
        {
            OpenEyesSmoothly();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isClosing = true;
            isOpening = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isClosing = false;
            isOpening = true;
        }
    }

    void OpenEyesSmoothly()
    {
        bool allOpen = true;

        for (int i = 0; i < topEye.Length; i++)
        {
            if (topEye[i] != null)
            {
                topEye[i].transform.position = Vector3.MoveTowards(
                    topEye[i].transform.position,
                    topeyeOrigin[i],
                    speed * Time.deltaTime
                );

                if (Vector3.Distance(topEye[i].transform.position, topeyeOrigin[i]) > 0.001f)
                    allOpen = false;
            }
        }

        for (int i = 0; i < bottomEye.Length; i++)
        {
            if (bottomEye[i] != null)
            {
                bottomEye[i].transform.position = Vector3.MoveTowards(
                    bottomEye[i].transform.position,
                    botEyeOrigin[i],
                    speed * Time.deltaTime
                );

                if (Vector3.Distance(bottomEye[i].transform.position, botEyeOrigin[i]) > 0.001f)
                    allOpen = false;
            }
        }

        if (allOpen)
            isOpening = false;
    }

    void CloseEyesSmoothly()
    {
        bool allClosed = true;

        for (int i = 0; i < topEye.Length; i++)
        {
            if (topEye[i] != null)
            {
                Vector3 targetPosition = topeyeOrigin[i] + Vector3.forward * closedZOffset;
                topEye[i].transform.position = Vector3.MoveTowards(
                    topEye[i].transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );

                if (Vector3.Distance(topEye[i].transform.position, targetPosition) > 0.001f)
                    allClosed = false;
            }
        }

        for (int i = 0; i < bottomEye.Length; i++)
        {
            if (bottomEye[i] != null)
            {
                Vector3 targetPosition = botEyeOrigin[i] + Vector3.forward * bottomClosedZOffset;
                bottomEye[i].transform.position = Vector3.MoveTowards(
                    bottomEye[i].transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );

                if (Vector3.Distance(bottomEye[i].transform.position, targetPosition) > 0.001f)
                    allClosed = false;
            }
        }

        if (allClosed)
            isClosing = false;
    }
}