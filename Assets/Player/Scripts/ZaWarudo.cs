using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ZaWarudo : MonoBehaviour
{
    public PlayerInputController inputControl; 

    public float maxScale = 200f;
    public float expandSpeed = 800f;
    public float ShrinkSpeed = 600f;
    public float delaySecond = 1;
    public float holdSecond = 2;

    public float scale = 0f;
    public bool isExpanding = false;
    public bool isShrinking = false;

    public float inputDio;

    public float scaleMam;

    private void Awake()
    {
        inputControl = new PlayerInputController();
    }
    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

 private void Start()
    {
       transform.localScale = new Vector3 (scale, scale, scale);

       

    }
    void Update()
    {
        inputDio =  inputControl.Gameplay.ZaWorldo.ReadValue<float>();

        if (inputDio == 1 && Time.timeScale == 1)
        {
            Invoke(nameof(Expand),  delaySecond); 
        }

        if (isExpanding)
        {
            scale += expandSpeed * Time.unscaledDeltaTime;
            transform.localScale = new Vector3(scale, scale, scale);

            if (scale > maxScale )
            {
                isExpanding = false;
                Invoke(nameof(Shrink), holdSecond);
            }
        }

        if (isShrinking)
        {
            scale -= ShrinkSpeed * Time.unscaledDeltaTime;

            if (scale <= 0 )
            {
                isExpanding = false;
                isShrinking = false;
                scale = 0; 
            }

            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    void Expand()
    {
        isExpanding = true;
        scale = 0;
    }

    void Shrink()
    {
        isShrinking = true;

    }
   

}
