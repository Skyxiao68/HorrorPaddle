using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneManager : MonoBehaviour
{

    public void play()
    {
        SceneManager.LoadSceneAsync (2);
    }
    public void Tutorial()
    {
        SceneManager.LoadSceneAsync (1);
    }
    public void Quit()
    {
        Application.Quit ();
    }  
}
