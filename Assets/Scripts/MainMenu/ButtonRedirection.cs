using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public void LoadNewScene(string _scene)
    {
        SceneManager.LoadSceneAsync(_scene);
    }
}
