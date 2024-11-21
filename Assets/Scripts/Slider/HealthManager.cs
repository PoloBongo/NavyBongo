using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CheckWinLoose checkWinLoose;
    private void OnEnable()
    {
        ReceiveDamage.OnHit += HandleHitPlayer;
    }

    private void OnDisable()
    {
        ReceiveDamage.OnHit -= HandleHitPlayer;
    }

    private void HandleHitPlayer(int _damage)
    {
        healthSlider.value -= _damage;
        if (healthSlider.value <= 0) checkWinLoose.LooseFunction();
    }
}
