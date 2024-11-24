using System.Collections;
using TMPro;
using UnityEngine;

public class PopupDamage : MonoBehaviour
{
    private Coroutine showPopupDamage;
    [SerializeField] private TMP_Text damageText;
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
        if (hasHit)
        {
            damageText.text = "Touché";
        }
        else
        {
            damageText.text = "Loupé";
        }
        showPopupDamage ??= StartCoroutine(ShowPopupDamage());
    }
    
    private IEnumerator ShowPopupDamage()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        showPopupDamage = null;
    }
}