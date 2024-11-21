using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBoat : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GameDataSave gameDataSave;
    [Header("Prefabs Boats")]
    [SerializeField] private List<GameObject> boats;

    private string actualBoatName;
    private Transform previousPosPlayerBoat;
    
    public delegate void OnFinishSpawnBoat();
    public static event OnFinishSpawnBoat OnInit;
    
    public delegate void OnUpdateTargetTower(GameObject _newPlayerBoat);
    public static event OnUpdateTargetTower OnUpdateTarget;
    
    private void Awake()
    {
        FindGameDataSave();
        if (!gameDataSave) Debug.LogError("GameSave not found in : " + gameObject.name);
        actualBoatName = "BoatA";
        GetActualBoat();
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject != null)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }

    private void GetActualBoat()
    {
        GameObject newBoat = null;

        switch (actualBoatName)
        {
            case "BoatA":
                newBoat = Instantiate(boats[0], previousPosPlayerBoat?.position ?? boats[0].transform.position, boats[0].transform.rotation);
                break;
            case "BoatB":
                newBoat = Instantiate(boats[1], previousPosPlayerBoat.position, previousPosPlayerBoat.rotation);
                break;
            case "BoatC":
                newBoat = Instantiate(boats[2], previousPosPlayerBoat.position, previousPosPlayerBoat.rotation);
                break;
        }

        if (newBoat == null)
        {
            Debug.LogError("Failed to instantiate new boat!");
            return;
        }

        // Assurez-vous que le bateau et ses composants sont prêts
        if (newBoat.GetComponent<BoatController>() == null)
        {
            Debug.LogError("New boat does not have a BoatController component!");
        }

        OnInit?.Invoke();
        OnUpdateTarget?.Invoke(newBoat);
    }
    
    private void OnEnable()
    {
        RedirectionGame.OnUpdateNewBoat += HandleUpdateBoat;
    }

    private void OnDisable()
    {
        RedirectionGame.OnUpdateNewBoat -= HandleUpdateBoat;
    }

    private void HandleUpdateBoat(string _boatName)
    {
        previousPosPlayerBoat = gameDataSave.GetActualPosPlayerBoat();
        actualBoatName = _boatName;
        GetActualBoat();
    }
}
