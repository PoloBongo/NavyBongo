using UnityEngine;
using UnityEngine.SceneManagement;

public class RedirectionGame : MonoBehaviour
{
    private GameDataSave gameDataSave;
    public delegate void OnUpdateBoat(string _boatName);
    public static event OnUpdateBoat OnUpdateNewBoat;
    
    public void CloseShop(string _boatName)
    {
        OnUpdateNewBoat?.Invoke(_boatName);
        SceneManager.UnloadSceneAsync("ShopInGame");
        Time.timeScale = 1f;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        if (GameDataSave.Instance != null && GameDataSave.Instance.GetStockPlayer() != null)
        {
            Destroy(GameDataSave.Instance.GetStockPlayer());
        }
    }
}
