using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class TimeManage : MonoBehaviour
{
    public static TimeManage Instance;

    [Header("Dio Time Settings")]
    public float dioTimeScale = 0.2f;
    public float dioTimeDuration = 3f;

    public System.Action<float> OnDioTimeScaleChanged;

    private void Awake()
    {
        if (Instance == null) 
        {
                Instance = this;
                DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    public void SetDioTimeScale(float newScale)
    {
        dioTimeScale = Mathf.Clamp(newScale, 0.01f, 1f);
        OnDioTimeScaleChanged?.Invoke(dioTimeScale);
    }

}
