using System;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{ 
    [Header("Equipment Settings")]
    [Range(0,50)] public int pistolAmmoCount = 0;
    [Range(0,100)] public int akAmmoCount = 0;
    [Range(0,6)] public int medkitCount = 0;

    public static ConsumableManager Instance;

    private void Awake()
    {
        Instance = this; 
    }

}
