using System.Collections;
using UnityEngine;

public class PopupMagnet : MonoBehaviour
{
    private Coroutine showPopupMagnet;
    [SerializeField] private GameObject showPopupMagnetGameObject;

    private void OnEnable()
    {
        CameraControl.OnActiveMagnetPopup += HandleObjectPopupMagnet;
    }

    private void OnDisable()
    {
        CameraControl.OnActiveMagnetPopup -= HandleObjectPopupMagnet;
    }

    private void HandleObjectPopupMagnet()
    {
        showPopupMagnet ??= StartCoroutine(ShowPopupHealth());
    }
    
    private IEnumerator ShowPopupHealth()
    {
        showPopupMagnetGameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        showPopupMagnetGameObject.SetActive(false);
        showPopupMagnet = null;
    }
}