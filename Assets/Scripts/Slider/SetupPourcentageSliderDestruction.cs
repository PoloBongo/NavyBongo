using UnityEngine;
using UnityEngine.UI;

public class SetupPourcentageSliderDestruction : MonoBehaviour
{
    private Slider slider;
    private GetAllTowersCount getAllTowersCount;

    private void Start()
    {
        slider = GetComponent<Slider>();
        getAllTowersCount = GetComponentInParent<GetAllTowersCount>();
        if (!slider) Debug.LogError("Slider Destruction cannot be null");
        if (!getAllTowersCount) Debug.LogError("GetAllTowersCount cannot be null");
        int newValue = getAllTowersCount.GetTowersCount() * 100;
        slider.maxValue = newValue;
    }
}
