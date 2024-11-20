using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    [Header("Listes des cameras")]
    public List<CinemachineVirtualCamera> cameras;
    
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
        //FindCameras();
    }
    
    private void OnEnable()
    {
        InstantiateBoat.OnInit += HandleInitControlsManager;
    }
    
    private void OnDisable()
    {
        InstantiateBoat.OnInit -= HandleInitControlsManager;
    }
    
    private void HandleInitControlsManager()
    {
        StartCoroutine(DelayedSetup());
    }

    private IEnumerator DelayedSetup()
    {
        yield return null;

        FindCameras();
    }
    
    private void FindCameras()
    {
        cameras.Clear();
        var foundCameras = FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None);
        foreach (var camera in foundCameras)
        {
            if (camera)
            {
                cameras.Add(camera);
            }
            else
            {
                Debug.LogWarning("Found a null OrientationCanon reference");
            }
        }
        
        totalNumbersOfCameras = cameras.Count - 1;
        SwitchPriorityCamera();
    }

    public void Initialize(PlayerInputAction _playerInputAction)
    {
        inputAction = _playerInputAction;
        inputAction.CameraController.Browse.performed += OnBrowsePerformed;
    }

    private void OnBrowsePerformed(InputAction.CallbackContext context)
    {
        if (this == null) return;
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
        CleanupCannonControllers();

        foreach (var t in controlsManager.cannonControllers)
        {
            if (!t) continue;
            var cannonFire = t.GetComponent<CannonFire>();
            if (cannonFire)
            {
                cannonFire.enabled = false;
            }
        }

        foreach (var t in controlsManager.cannonControllers)
        {
            if (!t) continue;
            if (t.name == _name)
            {
                var cannonFire = t.GetComponent<CannonFire>();
                if (cannonFire)
                {
                    cannonFire.enabled = true;
                }
            }
        }
    }
    
    private void CleanupCannonControllers()
    {
        controlsManager.cannonControllers.RemoveAll(item => item == null);
    }

}
