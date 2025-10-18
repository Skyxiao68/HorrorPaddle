
using UnityEngine;

public class ButtonBob : MonoBehaviour
{
    public float bobSpeed = 2f;
    public float bobHeight = 10f;
    private Vector2 startPos;

    void OnEnable()
    {
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x, newY);
    }
}