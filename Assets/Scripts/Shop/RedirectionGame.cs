using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectionGame : MonoBehaviour
{
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
        gameDataSave.AddBerrys(500);
        int berrysPlayer = gameDataSave.GetBerrys();
        int priceBoat = GetPriceOfBoat(_boatName);
        if (berrysPlayer >= priceBoat)
        {
            if (coroutinePopupShopS == null) coroutinePopupShopS = StartCoroutine(ShowPopupShopSuccess());
            gameDataSave.RemoveBerrys(priceBoat);
            OnUpdateNewBoat?.Invoke(_boatName);
            SceneManager.UnloadSceneAsync("ShopInGame");
            Time.timeScale = 1f;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
            if (GameDataSave.Instance != null && GameDataSave.Instance.GetStockPlayer() != null)
            {
                Destroy(GameDataSave.Instance.GetStockPlayer());
            }
        }
        else
        {
            if (coroutinePopupShopF == null) coroutinePopupShopF = StartCoroutine(ShowPopupShopFailed());
        }
    }

    private int GetPriceOfBoat(string _boatName)
    {
        switch (_boatName)
        {
            case "BoatA":
                return 100;
            case "BoatB":
                return 400;
            case "BoatC":
                return 1000;
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
}
