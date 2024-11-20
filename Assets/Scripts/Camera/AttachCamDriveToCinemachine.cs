using Cinemachine;
using UnityEngine;

public class AttachCamDriveToCinemachine : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private bool camDriveFound = false;

    private void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (!camDriveFound)
        {
            GameObject findCamDrive = GameObject.Find("CamDrive");
            if (findCamDrive)
            {
                vcam.Follow = findCamDrive.transform;
                camDriveFound = true;
                
                Destroy(this);
            }
        }
    }
}