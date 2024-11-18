using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionSplash;
    [SerializeField] private GameObject explosionFire;
    [SerializeField] private AudioSource explosionSound;
    private GameObject popupDamage;
    private bool stopUpdate = false;
    
    // event
    public delegate void OnDestroyAction(GameObject destroyedObject);
    public static event OnDestroyAction OnDestroyed;

    private void Start()
    {
        popupDamage = FindInactiveObjectByTag("PopupDamage");
    }

    GameObject FindInactiveObjectByTag(string _tag)
    {
        Transform[] allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform trans in allTransforms)
        {
            if (trans.hideFlags == HideFlags.None && trans.CompareTag(_tag))
            {
                return trans.gameObject;
            }
        }
        return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionFire, gameObject.transform.position, gameObject.transform.rotation);
        if (popupDamage) popupDamage.SetActive(true);
        explosionSound.Play();
        Destroy(gameObject); 
    }

    private void FixedUpdate()
    {
        if (stopUpdate) return;
        if (!(transform.position.y < 0)) return;
        Instantiate(explosionSplash, transform.position, transform.rotation);
        explosionSound.Play();
        Destroy(gameObject, 1f);
        stopUpdate = true;
    }
    
    public int GetDamage => damage;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(gameObject);
    }
}
