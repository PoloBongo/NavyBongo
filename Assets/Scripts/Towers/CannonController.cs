using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    [SerializeField] private Slider towerHealth;
    [SerializeField] private Slider uiDestruction;
    [SerializeField] private float maxHealth;
    [SerializeField] private float actualHealth;

    private TMP_Text destructionText;
    private GetAllTowersCount getAllTowersCount;

    private CannonAttack cannonAttack;
    private bool canRiposte;
    
    // data
    private GameDataSave gameDataSave;
    
    // event
    public delegate void OnDestroyAction(GameObject destroyedObject);
    public static event OnDestroyAction OnDestroyed;
    private void Start()
    {
        cannonAttack = GetComponent<CannonAttack>();
        if (!cannonAttack) Debug.LogError("CannonAttack cannot be null");
        cannonAttack.Initialize();
        canRiposte = false;
        getAllTowersCount = GetComponent<GetAllTowersCount>();
        if (!getAllTowersCount) Debug.LogError("GetAllTowersCount cannot be null");
        towerHealth.value = maxHealth;
        actualHealth = maxHealth;
        FindUIDestruction();
        FindGameDataSave();
    }

    private void FindUIDestruction()
    {
        GameObject uiDestructionGameObject = GameObject.FindGameObjectWithTag("UIDestruction");
        uiDestruction = uiDestructionGameObject.GetComponent<Slider>();
        if (uiDestruction == null) Debug.LogError("UI Destruction GameObject not found");
        destructionText = uiDestructionGameObject.GetComponentInChildren<TMP_Text>();
        if (destructionText == null) Debug.LogError("UI Destruction Text not found");
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            int actualDamage = other.gameObject.GetComponent<ReceiveDamage>().GetDamage;
            actualHealth -= actualDamage;
            UpdateUIDestructionPourcentage(actualDamage);
            UpdateHealthSlider();
            
            if (actualHealth < maxHealth) canRiposte = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cannonAttack.UpdateCheckRiposte(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cannonAttack.UpdateCheckRiposte(false);
        }
    }

    private void UpdateHealthSlider()
    {
        towerHealth.value = actualHealth;
        CheckHealthTower();
    }

    private void CheckHealthTower()
    {
        if (towerHealth.value <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateUIDestructionPourcentage(int _damage)
    {
        uiDestruction.value += _damage;
        float pourcentageDestruction = uiDestruction.value / getAllTowersCount.GetTowersCount();
        int pourcentageArrondi = Mathf.RoundToInt(pourcentageDestruction);
        destructionText.text = pourcentageArrondi + "% Destruction";
        
        gameDataSave.AddTotalPourcentageDestruction(pourcentageArrondi);
    }
    
    private void OnDestroy()
    {
        gameDataSave.AddBerrys(100);
        OnDestroyed?.Invoke(gameObject);
    }

    public bool GetCanRisposte()
    {
        return canRiposte;
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
