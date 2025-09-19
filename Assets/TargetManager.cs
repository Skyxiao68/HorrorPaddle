using System.Collections;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject target;
    public Light highLight;
    public Color coreectColor = Color.green;
    public Color wrongColor = Color.red;
    public float flashDuration = 2f;
    private bool wasCorrectHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Target number " + gameObject.name + " was hit");

            EasterEggManager manager = FindAnyObjectByType<EasterEggManager>();

            if (manager != null)
            {
                wasCorrectHit = manager.RegisterBall(target);

                if (highLight != null)
                {
                    Color flashColor = wasCorrectHit ? coreectColor : wrongColor;
                    StartCoroutine(FlashLight(flashColor));
                }
            }
          
        }
    }

    IEnumerator FlashLight(Color color) 
    {
      
        Color originalColor = highLight.color; 
        highLight.color = color;
        highLight.enabled = true;

        yield return new WaitForSeconds(flashDuration);

      
        highLight.enabled = false;
        highLight.color = originalColor;
    }
}