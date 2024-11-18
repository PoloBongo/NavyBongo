using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllTowersCount : MonoBehaviour
{
    private int allTowersCount;

    private void Awake()
    {
        FindAllTowers();
    }

    private void FindAllTowers()
    {
        var allTowerControllers = FindObjectsByType<TowerController>(FindObjectsSortMode.None);
        allTowersCount = allTowerControllers.Length;
    }

    public int GetTowersCount()
    {
        return allTowersCount;
    }
}
