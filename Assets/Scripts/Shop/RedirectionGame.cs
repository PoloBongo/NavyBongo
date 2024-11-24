using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectionGame : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceS;
    [SerializeField] private AudioSource audioSourceF;
    private GameDataSave gameDataSave;
    [SerializeField] private GameObject popupShopSuccess;
    [SerializeField] private GameObject popupShopFailed;
    private Coroutine coroutinePopupShopS;
    private Coroutine coroutinePopupShopF;
    public delegate void OnUpdateBoat(string _boatName);
    public static event OnUpdateBoat OnUpdateNewBoat;

    private void Start()
    {
        FindGameDataSave();
    }

    public void BuyNewBoat(string _boatName)
    {
        int berrysPlayer = gameDataSave.GetBerrys();
        int priceBoat = GetPriceOfBoat(_boatName);
        if (berrysPlayer >= priceBoat)
        {
            audioSourceS.Play();
            if (coroutinePopupShopS == null) coroutinePopupShopS = StartCoroutine(ShowPopupShopSuccess());
            gameDataSave.RemoveBerrys(priceBoat);
            gameDataSave.SetStockPlayerName(_boatName);
            OnUpdateNewBoat?.Invoke(_boatName);
            SceneManager.UnloadSceneAsync("ShopInGame");
            Time.timeScale = 1f;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
            if (gameDataSave.GetStockPlayer())
            {
                Debug.Log("destro boat " + gameDataSave.GetStockPlayer().name);
                Destroy(gameDataSave.GetStockPlayer());
            }
            else
            {
                Debug.Log("dont destro boat ");
            }
        }
        else
        {
            audioSourceF.Play();
            if (coroutinePopupShopF == null) coroutinePopupShopF = StartCoroutine(ShowPopupShopFailed());
        }
    }

    private int GetPriceOfBoat(string _boatName)
    {
        switch (_boatName)
        {
            case "BoatA":
                return 0;
            case "BoatB":
                return 200;
            case "BoatC":
                return 400;
        }

        return -1;
    }
    
    private IEnumerator ShowPopupShopSuccess()
    {
        popupShopSuccess.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupShopSuccess.SetActive(false);
        coroutinePopupShopS = null;
    }
    
    private IEnumerator ShowPopupShopFailed()
    {
        popupShopFailed.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupShopFailed.SetActive(false);
        coroutinePopupShopF = null;
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }

    public void BackToGame()
    {
        audioSourceS.Play();
        SceneManager.UnloadSceneAsync("ShopInGame");
        Time.timeScale = 1f;
        gameDataSave.GetPlayerInputAction().Enable();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }
}
