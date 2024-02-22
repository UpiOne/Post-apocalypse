using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Medkit Item", menuName = "Inventory/Medkit Item")]
[System.Serializable]
public class InventoryMedkit : InventoryItem
{
    [Header("Medkit Settings")]
    [Range(0,100)]
    public int hpRestore = 0;
    public void Awake()
    {
        itemType = ItemType.Medkit;
    }
}
