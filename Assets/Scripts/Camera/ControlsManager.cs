using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    public CameraControl cameraControl;
    public BoatController boatController;
    public List<OrientationCanon> cannonControllers;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();

        boatController.Initialize(playerInputAction);
        cannonControllers[0].Initialize(playerInputAction);
        cannonControllers[1].Initialize(playerInputAction);
        cameraControl.Initialize(playerInputAction);
        
        playerInputAction.Disable();

        ActivateBoatControls();
        ActivateCameraControls();
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
}