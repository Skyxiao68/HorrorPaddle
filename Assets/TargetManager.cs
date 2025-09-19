using System.Collections;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject target;
    public Light highLight; // Your variable is named 'highLight' (capital L)
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

                if (highLight != null) // Use 'highLight' (matches your public variable)
                {
                    Color flashColor = wasCorrectHit ? coreectColor : wrongColor;
                    StartCoroutine(FlashLight(flashColor));
                }
            }
            else // This else should be for when manager is null, not part of the tag check
            {
                if (highLight != null)
                {
                    StartCoroutine(FlashLight(Color.white));
                }
            }
        }
    }

    IEnumerator FlashLight(Color color) // Fixed parameter syntax: 'Color color' not 'Color.color'
    {
        // Store original color
        Color originalColor = highLight.color; // Use 'highLight' (your variable name)

        // Set new color and enable light
        highLight.color = color;
        highLight.enabled = true;

        yield return new WaitForSeconds(flashDuration);

        // Revert to original color and disable light
        highLight.enabled = false;
        highLight.color = originalColor;
    }
}