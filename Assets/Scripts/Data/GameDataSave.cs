using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CannonData
{
    public GameObject gameObject;
    public bool isReloading;
}

public class GameDataSave : MonoBehaviour
{
    private static GameDataSave instance;
    
    public static GameDataSave Instance
    {
        get
        {
            if (!instance)
            {
                instance = (GameDataSave)FindFirstObjectByType(typeof(GameDataSave));

                if (!instance)
                {
                    Debug.LogError("No GameDataSave found in the scene!");
                }
            }

            return instance;
        }
    }

    private int totalCannonFire;
    private int totalPourcentageDestruction;
    private int berrys;
    private Transform posPlayerBoat;
    private GameObject stockPlayer;
    private string stockPlayerName;
    private PlayerInputAction playerInputAction;
    [SerializeField]
    private List<CannonData> cannonGameObjects = new List<CannonData>();

    private int max = 100;
    private int min = 0;
    private int currentAim = 100;
    private int successfullAim = 0;
    
    private void Awake()
    {
        stockPlayerName = "BoatA";
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
        return currentAim;
    }
    
    private void CalculateAim() {
        if (totalCannonFire > 0)
        {
            currentAim = (successfullAim * 100) / totalCannonFire;
        } 
        else 
        { 
            currentAim = 100;
            
        }
        currentAim = Mathf.Clamp(currentAim, min, max); 
    }

    public void AddTotalAim()
    {
        successfullAim++;
        CalculateAim();
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

    public string GetStockPlayerName()
    {
        return stockPlayerName;
    }

    public void SetStockPlayerName(string _newStockPlayerName)
    {
        stockPlayerName = _newStockPlayerName;
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputAction;
    }

    public void SetPlayerInputAction(PlayerInputAction _newPlayerInputAction)
    {
        playerInputAction = _newPlayerInputAction;
    }

    public void SetReloadingBool(GameObject _obj, bool _isReloading)
    {
        cannonGameObjects.RemoveAll(cannon => cannon.gameObject == null);
        bool found = false;

        foreach (var cannon in cannonGameObjects)
        {
            if (cannon.gameObject.name == _obj.name)
            {
                cannon.isReloading = _isReloading;
                Debug.Log(_obj.name + " is reloading " + _isReloading);
                found = true;
                break;
            }
        }

        if (!found)
        {
            AddCannon(_obj, _isReloading);
        }
    }
    
    public void AddCannon(GameObject _gameObject, bool _isReloading)
    {
        cannonGameObjects.Add(new CannonData { gameObject = _gameObject, isReloading = _isReloading });
    }

    public bool GetReloadingBool(GameObject obj)
    {
        if (cannonGameObjects.Count > 0)
        {
            foreach (var cannon in cannonGameObjects)
            {
                if (cannon.gameObject == obj)
                {
                    return cannon.isReloading;
                }
            }

            // Si l'objet n'est pas trouvé, renvoyer une valeur par défaut.
            return false;
        }
        return false;
    }
}
