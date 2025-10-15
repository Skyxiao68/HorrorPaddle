using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene4 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(4);
        }
    }
}
