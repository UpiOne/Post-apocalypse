using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipmentType
{
    Clothing,
    Helmet
}

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Equipment Item")]
[System.Serializable]
public class InventoryEquipment : InventoryItem
{
    [Header("Equipment Settings")]
    [Range(0,100)]
    public int protection = 0;
    public EquipmentType equipmentType;
    public void Awake()
    {
        itemType = ItemType.Equipment;
    }
}
