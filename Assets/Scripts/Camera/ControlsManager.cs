using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    [SerializeField] private GameDataSave gameDataSave;
    private PlayerInputAction playerInputAction;
    private CameraControl cameraControl;
    private BoatController boatController;
    public List<OrientationCanon> cannonControllers;
    public ShopControls shopControls;
    
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
        FindOrientationCanon();
    }

    public void ActivateBoatControls()
    {
        playerInputAction.Cannon.Disable();
        playerInputAction.Movement.Enable();
    }

    public void ActivateCannonControls()
    {
        playerInputAction.Cannon.Enable();
        playerInputAction.Movement.Disable();
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