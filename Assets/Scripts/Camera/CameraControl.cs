using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    [Header("Listes des cameras")]
    [SerializeField] private List<CinemachineVirtualCamera> cameras;
    
    private ControlsManager controlsManager;
    
    private PlayerInputAction inputAction;
    private int indexCamera;
    private int maximumPriority;
    private int minimumPriority;
    private int totalNumbersOfCameras;

    private void Start()
    {
        controlsManager = GetComponent<ControlsManager>();
        indexCamera = 0;
        maximumPriority = 10;
        minimumPriority = 5;
        totalNumbersOfCameras = cameras.Count - 1;
        SwitchPriorityCamera();
    }

    public void Initialize(PlayerInputAction _playerInputAction)
    {
        inputAction = _playerInputAction;
        inputAction.CameraController.Browse.performed += OnBrowsePerformed;
        inputAction.CameraController.Browse.canceled += OnBrowseCanceled;
    }

    private void OnBrowsePerformed(InputAction.CallbackContext context)
    {
        float browseValue = context.ReadValue<float>();
        switch (browseValue)
        {
            case > 0:
                indexCamera++;
                indexCamera = Mathf.Clamp(indexCamera, 0, totalNumbersOfCameras);
                SwitchPriorityCamera();
                break;
            case < 0:
                indexCamera--;
                indexCamera = Mathf.Clamp(indexCamera, 0, totalNumbersOfCameras);
                SwitchPriorityCamera();
                break;
        }
    }
    
    private void OnBrowseCanceled(InputAction.CallbackContext context)
    {
        
    }

    private void SwitchPriorityCamera()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            if (i == indexCamera)
            {
                cameras[i].Priority = maximumPriority;
                ManagerActivatedCannon(cameras[i].name);
            }
            else
            {
                cameras[i].Priority = minimumPriority;
            }
        }

        SwitchInputController();
    }
    
    private void SwitchInputController()
    {
        switch (cameras[indexCamera].gameObject.tag)
        {
            case "CameraDrive":
                controlsManager.ActivateBoatControls();
                break;
            case "Cannon":
                controlsManager.ActivateCannonControls();
                break;
            default:
                controlsManager.ActivateBoatControls();
                break;
        }
    }

    private void ManagerActivatedCannon(string _name)
    {
        for (int i = 0; i < controlsManager.cannonControllers.Count; i++)
        {
            controlsManager.cannonControllers[i].GetComponent<CannonFire>().enabled = controlsManager.cannonControllers[i].name == _name;
        }
    }
}
