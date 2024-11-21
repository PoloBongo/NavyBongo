using System.Collections;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    private Coroutine showPopupDamage;
    private void OnEnable()
    {
        ReceiveDamage.OnDestroyed += HandleObjectDestruction;
    }

    private void OnDisable()
    {
        ReceiveDamage.OnDestroyed -= HandleObjectDestruction;
    }

    private void HandleObjectDestruction(GameObject destroyedObject, bool hasHit)
    {
        if (hasHit) showPopupDamage ??= StartCoroutine(ShowPopupDamage());
    }
    
    private IEnumerator ShowPopupDamage()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        showPopupDamage = null;
    }
}