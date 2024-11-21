using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddHealthToBoat : MonoBehaviour
{
    [Header("Object Throwable Settings")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private int addHealth;
    [SerializeField] private GameObject popupHealth;
    private Coroutine coroutinePopupHealth;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float newHealth = healthSlider.value;
            float targetHealth = newHealth += addHealth;
            if (targetHealth > 100)
            {
                healthSlider.value = 100;
            }
            else
            {
                if (coroutinePopupHealth == null ) coroutinePopupHealth = StartCoroutine(ShowPopupAddHealth());
                healthSlider.value = targetHealth;
            }
            
            Destroy(gameObject);
        }
    }
    
    private IEnumerator ShowPopupAddHealth()
    {
        popupHealth.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupHealth.SetActive(false);
        coroutinePopupHealth = null;
    }
}
