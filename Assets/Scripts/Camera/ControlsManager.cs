using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    [SerializeField] private GameDataSave gameDataSave;
    private PlayerInputAction playerInputAction;
    private CameraControl cameraControl;
    private BoatController boatController;
    private MagnetController magnetController;
    public List<OrientationCanon> cannonControllers;
    public ShopControls shopControls;

    private void Awake()
    {
        FindGameDataSave();
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject != null)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }

    private void OnEnable()
    {
        playerInputAction = new PlayerInputAction();
        gameDataSave.SetPlayerInputAction(playerInputAction);
        InstantiateBoat.OnInit += HandleInitControlsManager;
    }
    
    private void HandleInitControlsManager()
    {
        StartCoroutine(DelayedSetup());
    }

    private IEnumerator DelayedSetup()
    {
        yield return null;

        SetupSerializableFields();

        if (!boatController)
        {
            Debug.LogError("BoatController is still null after setup!");
        }

        boatController?.Initialize(playerInputAction);
        cameraControl?.Initialize(playerInputAction);
        shopControls?.Initialize(playerInputAction);
        magnetController?.Initialize(playerInputAction);

        foreach (var t in cannonControllers)
        {
            if (!t)
            {
                Debug.LogWarning("Cannon controller is null");
                continue;
            }
            t.Initialize(playerInputAction);
        }

        playerInputAction.Disable();

        ActivateBoatControls();
        ActivateCameraControls();
        ActivateShopControls();
    }

    private void FindCameraControl()
    {
        CameraControl foundCameraControl = (CameraControl)FindAnyObjectByType(typeof(CameraControl));
        cameraControl = foundCameraControl;
    }
    
    private void FindBoatController()
    {
        BoatController foundBoatController = (BoatController)FindAnyObjectByType(typeof(BoatController));
        boatController = foundBoatController;
        if (!boatController) Debug.LogError("BoatController not found in " + gameObject.name);
    }
    
    private void FindMagnetController()
    {
        MagnetController foundMagnetController = (MagnetController)FindAnyObjectByType(typeof(MagnetController));
        magnetController = foundMagnetController;
        if (!magnetController) Debug.LogError("MagnetController not found in " + gameObject.name);
    }
    
    private void FindOrientationCanon()
    {
        cannonControllers.Clear();
        var foundOrientationCanon = FindObjectsByType<OrientationCanon>(FindObjectsSortMode.None);
        foreach (var canon in foundOrientationCanon)
        {
            if (canon)
            {
                cannonControllers.Add(canon);
            }
            else
            {
                Debug.LogWarning("Found a null OrientationCanon reference");
            }
        }
    }

    private void SetupSerializableFields()
    {
        FindCameraControl();
        FindBoatController();
        FindMagnetController();
        FindOrientationCanon();
    }

    public void ActivateBoatControls()
    {
        playerInputAction.Cannon.Disable();
        playerInputAction.Movement.Enable();
        playerInputAction.Magnet.Disable();
    }

    public void ActivateCannonControls()
    {
        Debug.Log("activate");
        playerInputAction.Cannon.Enable();
        playerInputAction.Movement.Disable();
        playerInputAction.Magnet.Disable();
    }
    
    public void ActivateMagnetControls()
    {
        Debug.Log("activateionnnnnnnn");
        playerInputAction.Magnet.Enable();
        playerInputAction.Movement.Disable();
        playerInputAction.Cannon.Disable();
    }
    
    private void ActivateCameraControls()
    {
        playerInputAction.CameraController.Enable();
    }

    private void ActivateShopControls()
    {
        playerInputAction.Shop.Enable();
    }

    public PlayerInputAction GetPlayerInputAction()
    {
        return playerInputAction;
    }

    private void OnDisable()
    {
        InstantiateBoat.OnInit -= HandleInitControlsManager;
    }

    private void HandleUpdateBoat()
    {
        SetupSerializableFields();
    }

    public List<OrientationCanon> GetCannonControllers()
    {
        return cannonControllers;
    }
}