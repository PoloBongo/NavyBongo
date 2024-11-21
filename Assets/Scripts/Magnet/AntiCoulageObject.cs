using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class AntiCoulageObject : MonoBehaviour
{
    [SerializeField] private FitToWaterSurface fitToWaterSurface;
    [SerializeField] private WaterSurface waterSurface;
    [SerializeField] private Rigidbody rigidbody;
    private void FixedUpdate()
    {
        if (gameObject.transform.position.y < 0)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            fitToWaterSurface.targetSurface = waterSurface;
        }
        else if (gameObject.transform.position.y > 1)
        {
            rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
            fitToWaterSurface.targetSurface = null;
        }
    }
}
