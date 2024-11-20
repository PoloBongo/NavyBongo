using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
