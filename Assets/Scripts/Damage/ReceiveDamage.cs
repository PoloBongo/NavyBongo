using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionSplash;
    [SerializeField] private GameObject explosionFire;
    [SerializeField] private AudioSource explosionSound;
    [SerializeField] AudioClip[] explosions; 
    private GameObject popupDamage;
    private bool stopUpdate = false;
    
    // data
    private GameDataSave gameDataSave;

    private bool antiDupliData;
    // event
    public delegate void OnDestroyAction(GameObject destroyedObject);
    public static event OnDestroyAction OnDestroyed;

    private void Start()
    {
        popupDamage = FindInactiveObjectByTag("PopupDamage");
        antiDupliData = true;
        FindGameDataSave();
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
        CreateSound(1);
        Destroy(gameObject);

        if (!collision.gameObject.CompareTag("Enemy"))
        {
            gameDataSave.AddTotalAim(-10);
            antiDupliData = true;
        }
        else
        {
            gameDataSave.AddTotalAim(10);
        }
    }

    private void FixedUpdate()
    {
        if (stopUpdate) return;
        if (!(transform.position.y < 0)) return;
        Instantiate(explosionSplash, transform.position, transform.rotation);
        CreateSound(0);
        if (!antiDupliData) gameDataSave.AddTotalAim(-10);
        Destroy(gameObject, 1f);
        stopUpdate = true;
    }
    
    public int GetDamage => damage;

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(gameObject);
    }

    private void CreateSound(int _index)
    {
        GameObject soundEffectCannon = new GameObject();
        soundEffectCannon.transform.position = transform.position;
        soundEffectCannon.AddComponent<AutoDestruction>();
        AudioSource clip = soundEffectCannon.AddComponent<AudioSource>();
        clip.pitch = 1.3f;
        clip.spatialBlend = 1f;
        clip.clip = explosions[_index];
        clip.Play();
    }
    
    private void FindGameDataSave()
    {
        GameObject gameDataSaveGameObject = GameObject.FindGameObjectWithTag("GameDataSave");
        if (gameDataSaveGameObject != null)
            gameDataSave = gameDataSaveGameObject.GetComponent<GameDataSave>();
        else
            Debug.LogError("GameDataSave not found!");
    }
}
