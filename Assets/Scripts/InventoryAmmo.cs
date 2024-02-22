using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AmmoType
{
    Pistol,
    Riffle

}
[CreateAssetMenu(fileName = "New Ammo Item", menuName = "Inventory/Ammo Item")]
[System.Serializable]
public class InventoryAmmo : InventoryItem
{
    [Header("Ammo Settings")]
    [Range(0,100)]
    public float weightPerAmmo = 0.01f; 
    public AmmoType ammoType;
    public void Awake()
    {
        itemType = ItemType.Ammo;
    }
}
