using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private Slider volumeSlider;
    private float currentVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        currentVolume = volumeSlider.value;
    }

    private void Start()
    {
        ApplyVolume();
    }

    public void OnVolumeSliderChanged()
    {
        currentVolume = volumeSlider.value;
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        AudioListener.volume = currentVolume;
    }
    
    public void ApplyVolumeNew(float _value)
    {
        currentVolume = _value;
        ApplyVolume();
    }

    public void SetNewSlider(Slider slider)
    {
        volumeSlider = slider;
    }

    public float GetVolume()
    {
        return currentVolume;
    }
}