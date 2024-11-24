using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddHealthToBoat : MonoBehaviour
{
    [Header("Object Throwable Settings")]
    [SerializeField] private int addHealth;
    private Slider healthSlider;
    private Coroutine coroutinePopupHealth;
    
    public delegate void OnHitWood(bool _isFull);
    public static event OnHitWood OnHitWoodEvent;

    private void OnCollisionEnter(Collision other)
    {
        if (!healthSlider) FoundPlayerUIHealth();
        
        if (other.gameObject.CompareTag("Player"))
        {
            float currentHealth = healthSlider.value;
            float newHealth = Mathf.Clamp(currentHealth + addHealth, 0, 100);

            healthSlider.value = newHealth;

            if (newHealth == 100 && currentHealth == 100)
            {
                OnHitWoodEvent?.Invoke(true);
            }
            else
            {
                OnHitWoodEvent?.Invoke(false);
            }

            Destroy(gameObject, 1);
        }

    }

    private void FoundPlayerUIHealth()
    {
        GameObject playerUI = GameObject.FindGameObjectWithTag("UIPlayerHealth");
        healthSlider = playerUI.GetComponent<Slider>();
    }
}
