using System.Security.AccessControl;
using UnityEngine;

public class TimeManage : MonoBehaviour
{
    public static TimeManage instance;


    [Header("Time Setting")]
    public float slowDownFactor = 0.1f;
    public float slowDownLengh = 2f;

    private bool isSlowingDown = false;
    private float originalFixedDeltaTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        originalFixedDeltaTime = Time.fixedDeltaTime; 
    }
    void Update()
    {
       if (!isSlowingDown)
        {
            Time.timeScale += (1f / slowDownLengh) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
            
            if (Time.timeScale >= 0.99f)
            {
                Time.timeScale = 1f;
                isSlowingDown = false; 
            }
        }

    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale; 
        isSlowingDown = true;
    }

    public void ReturnToNormalTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isSlowingDown = false; 
    }
}
