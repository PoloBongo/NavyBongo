using UnityEngine;

public class GameDataSave : MonoBehaviour
{
    private static GameDataSave instance;
    
    public static GameDataSave Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GameDataSave)FindFirstObjectByType(typeof(GameDataSave));

                if (instance == null)
                {
                    Debug.LogError("No GameDataSave found in the scene!");
                }
            }

            return instance;
        }
    }

    private int totalCannonFire;
    private int totalPourcentageDestruction;
    private int totalAim;
    private int berrys;
    private Transform posPlayerBoat;
    private GameObject stockPlayer;
    private PlayerInputAction playerInputAction;

    private int max = 100;
    private int min = 0;
    
    private void Awake()
    {
        totalAim = 100;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameObject.tag = "GameDataSave";
        DontDestroyOnLoad(gameObject);
    }
    
    public int GetTotalCannonFire()
    {
        return totalCannonFire;
    }

    public void AddTotalCannonFire()
    {
        totalCannonFire++;
        if (totalCannonFire < min) totalCannonFire = min;
    }

    public int GetTotalPourcentageDestruction()
    {
        return totalPourcentageDestruction;
    }

    public void AddTotalPourcentageDestruction(int _value)
    {
        totalPourcentageDestruction += _value;
        if (totalPourcentageDestruction > max) totalPourcentageDestruction = max;
        if (totalPourcentageDestruction < min) totalPourcentageDestruction = min;
    }

    public int GetTotalAim()
    {
        return totalAim;
    }

    public void AddTotalAim(int _value)
    {
        totalAim += _value;
        if (totalAim > max) totalAim = max;
        if (totalAim < min) totalAim = min;
    }

    public int GetBerrys()
    {
        return berrys;
    }

    public void AddBerrys(int _value)
    {
        berrys += _value;
    }
    
    public void RemoveBerrys(int _value)
    {
        berrys -= _value;
    }

    public Transform GetActualPosPlayerBoat()
    {
        return posPlayerBoat;
    }

    public void SetActualPosPlayerBoat(Transform _newPosPlayerBoat)
    {
        posPlayerBoat = _newPosPlayerBoat;
    }

    public GameObject GetStockPlayer()
    {
        return stockPlayer;
    }

    public void SetStockPlayer(GameObject _newStockPlayer)
    {
        stockPlayer = _newStockPlayer;
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputAction;
    }

    public void SetPlayerInputAction(PlayerInputAction _newPlayerInputAction)
    {
        playerInputAction = _newPlayerInputAction;
    }
}
