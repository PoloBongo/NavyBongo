using TMPro;
using UnityEngine;

public class UpdateBerrys : MonoBehaviour
{
    private GameDataSave gameDataSave;
    [Header("Berrys")]
    [SerializeField] private TMP_Text berrysText;
    
    private void Start()
    {
        FindGameDataSave();
        if (!gameDataSave) Debug.LogError("GameDataSave not found");

        UpdateBerrysText();
    }
    
    private void OnEnable()
    {
        TowerController.OnDestroyed += HandleObjectDestruction;
        RedirectionGame.OnUpdateNewBoat += HandleObjectDestruction2;
    }

    private void OnDisable()
    {
        RedirectionGame.OnUpdateNewBoat -= HandleObjectDestruction2;
    }

    private void HandleObjectDestruction(GameObject destroyedObject)
    {
        UpdateBerrysText();
    }
    
    private void HandleObjectDestruction2(string _boatName)
    {
        UpdateBerrysText();
    }

    private void UpdateBerrysText()
    {
        int totalBerrys = gameDataSave.GetBerrys();
        berrysText.text = totalBerrys.ToString();
    }

    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject != null)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }
}