using TMPro;
using UnityEngine;

public class StatisticsPut : MonoBehaviour
{
    private GameDataSave gameDataSave;
    [Header("Statistics")]
    [SerializeField] private TMP_Text cannonFireText;
    [SerializeField] private TMP_Text shotAimText;
    [SerializeField] private TMP_Text pourcentageDamageText;

    private void Start()
    {
        FindGameDataSave();
        if (!gameDataSave) Debug.LogError("GameDataSave not found");
        
        int totalCannonFire = gameDataSave.GetTotalCannonFire();
        cannonFireText.text = "Cannon Shot: " + totalCannonFire;
        
        int totalShotAim = gameDataSave.GetTotalAim();
        shotAimText.text = "Shot Aim: " + totalShotAim + "%";
        
        int totalDamage = gameDataSave.GetTotalPourcentageDestruction();
        pourcentageDamageText.text = "Total Damage: " + totalDamage + "%";
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
