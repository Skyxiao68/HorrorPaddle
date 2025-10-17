using UnityEngine;

public class ThemeSongPlay : MonoBehaviour
{
    private AudioSource mainMenuSource;
    public AudioClip mainMenuTheme;

    void Start()
    {
        mainMenuSource = GetComponent<AudioSource>();

        mainMenuSource.clip = mainMenuTheme;
        mainMenuSource.loop=true;


        mainMenuSource.Play();
    }

   
}
