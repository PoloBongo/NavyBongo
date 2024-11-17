using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
    [SerializeField] private Slider towerHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float actualHealth;

    private void Start()
    {
        towerHealth.value = maxHealth;
        actualHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            Debug.Log("sa touche chef");
            int actualDamage = other.gameObject.GetComponent<ReceiveDamage>().GetDamage;
            actualHealth -= actualDamage;
        }
    }
}
