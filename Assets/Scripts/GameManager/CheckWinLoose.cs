using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckWinLoose : MonoBehaviour
{
    private GetAllTowersCount getAllTowersCount;
    private int towersCount;
    private int towerAlreadyDestroy;
    [SerializeField] private ControlsManager controlsManager;

    private void Start()
    {
        getAllTowersCount = GetComponent<GetAllTowersCount>();
        if (!getAllTowersCount) Debug.LogError("GetAllTowersCount not found");
        towersCount = getAllTowersCount.GetTowersCount();
    }

    private void OnEnable()
    {
        TowerController.OnDestroyed += HandleObjectDestruction;
    }

    private void OnDisable()
    {
        TowerController.OnDestroyed -= HandleObjectDestruction;
    }

    private void HandleObjectDestruction(GameObject destroyedObject)
    {
        towerAlreadyDestroy++;
        CheckWinLooseFunc();
    }

    private void CheckWinLooseFunc()
    {
        if (towerAlreadyDestroy == towersCount)
        {
            StartCoroutine(LoadScene("Win"));
        }
    }

    private IEnumerator LoadScene(string _winLoose)
    {
        controlsManager.GetPlayerInputAction().Disable();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(_winLoose);
    }

    public void LooseFunction()
    {
        StartCoroutine(LoadScene("Loose"));
    }
}