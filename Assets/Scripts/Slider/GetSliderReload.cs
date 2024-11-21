using System;
using UnityEngine;
using UnityEngine.UI;

public class GetSliderReload : MonoBehaviour
{
    private Slider slider;
    private CannonFire cannonFire;

    private void Awake()
    {
        cannonFire = GetComponent<CannonFire>();
        slider = FindSliderInCannon();
        if (!slider) Debug.LogError("Cannon slider not found in " + gameObject.name);
        cannonFire.SetSliderReload(slider);
    }

    private Slider FindSliderInCannon()
    {
        Slider[] allSliders = Resources.FindObjectsOfTypeAll<Slider>();
        foreach (Slider slider in allSliders)
        {
            if (slider.gameObject.CompareTag("ReloadCannon")) return slider;
        }
        return null;
    }
}
