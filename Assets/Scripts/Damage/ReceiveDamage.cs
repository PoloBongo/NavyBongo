using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour
{
    [SerializeField] private int damage;

    public event Action OnDestroyed;
    
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private AudioSource explosionSound;
    private bool stopUpdate = false;
    private void OnCollisionEnter(Collision collision)
    {
        OnDestroyed?.Invoke();
        Destroy(gameObject); 
    }

    private void FixedUpdate()
    {
        if (stopUpdate) return;
        if (!(transform.position.y < 0)) return;
        Instantiate(explosion, transform.position, transform.rotation);
        explosionSound.Play();
        stopUpdate = true;
    }
    
    public int GetDamage => damage;
}
