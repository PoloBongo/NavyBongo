using UnityEngine;
using UnityEngine.UI;

public class SetupSliderSound : MonoBehaviour
{
    [SerializeField] private Slider mainSlider;
    private AudioManager audioManager;
    void Start()
    {
        audioManager = AudioManager.Instance;
        audioManager.SetNewSlider(mainSlider);
        mainSlider.value = audioManager.GetVolume();
        mainSlider.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
    }

    public void ValueChangeCheck()
    {
        audioManager.ApplyVolumeNew(mainSlider.value);
    }
}
