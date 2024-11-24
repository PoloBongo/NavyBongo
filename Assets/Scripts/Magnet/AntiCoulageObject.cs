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
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private List<Material> materials;

    private void Start()
    {
        if (!waterSurface) FoundWaterSurface();
    }

    private void FixedUpdate()
    {
        if (gameObject.transform.position.y < 0)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            fitToWaterSurface.targetSurface = waterSurface;
            meshRenderer.material = materials[1];
        }
        else if (gameObject.transform.position.y > 1)
        {
            rigidbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
            fitToWaterSurface.targetSurface = null;
            meshRenderer.material = materials[0];
        }
    }

    private void FoundWaterSurface()
    {
        var foundWaterSurface = FindObjectsByType<WaterSurface>(FindObjectsSortMode.None);
        waterSurface = foundWaterSurface[0];
    }
}
