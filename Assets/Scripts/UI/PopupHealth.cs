using System.Collections;
using TMPro;
using UnityEngine;

public class PopupHealth : MonoBehaviour
{
    private Coroutine showPopupHealth;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private GameObject gameObjectPopup;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        AddHealthToBoat.OnHitWoodEvent += HandleObjectAddHealth;
    }

    private void OnDisable()
    {
        AddHealthToBoat.OnHitWoodEvent -= HandleObjectAddHealth;
    }

    private void HandleObjectAddHealth(bool _isFalse)
    {
        gameObjectPopup.SetActive(true);
        showPopupHealth ??= StartCoroutine(ShowPopupHealth(_isFalse));
    }
    
    private IEnumerator ShowPopupHealth(bool _isFalse)
    {
        gameObject.SetActive(true);
        popupText.text = !_isFalse ? "Tu viens de réparer ton bateau" : "Ton bateau a déjà toute sa vie";
        if (!audioSource.isPlaying) audioSource.Play();
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        gameObjectPopup.SetActive(false);
        showPopupHealth = null;
    }
}