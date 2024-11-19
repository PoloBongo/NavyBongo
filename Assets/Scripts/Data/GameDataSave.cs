using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameDataSave : MonoBehaviour
{
    private static GameDataSave Instance;

    private int totalCannonFire;
    private int totalPourcentageDestruction;
    private int totalAim;

    private int max = 100;
    private int min = 0;

    private void Awake()
    {
        totalAim = 100;
        if (!Instance)
        {
            Instance = this;
            gameObject.tag = "GameDataSave";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public int GetTotalCannonFire()
    {
        return totalCannonFire;
    }

    public void AddTotalCannonFire()
    {
        totalCannonFire++;
        if (totalCannonFire < min) totalCannonFire = min;
    }

    public int GetTotalPourcentageDestruction()
    {
        return totalPourcentageDestruction;
    }

    public void AddTotalPourcentageDestruction(int _value)
    {
        totalPourcentageDestruction += _value;
        if (totalPourcentageDestruction > max) totalPourcentageDestruction = max;
        if (totalPourcentageDestruction < min) totalPourcentageDestruction = min;
    }

    public int GetTotalAim()
    {
        return totalAim;
    }

    public void AddTotalAim(int _value)
    {
        totalAim += _value;
        if (totalAim > max) totalAim = max;
        if (totalAim < min) totalAim = min;
    }
}
