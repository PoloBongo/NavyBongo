using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AttachCamFireToCinemachine : MonoBehaviour
{
    private CinemachineFreeLook vcam;

    private void Start()
    {
        vcam = GetComponent<CinemachineFreeLook>();
    }
}