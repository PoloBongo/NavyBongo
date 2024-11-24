using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void LoadNewScene(string _scene)
    {
        audioSource.Play();
        SceneManager.LoadSceneAsync(_scene);
    }
}
