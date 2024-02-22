using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Inventory Settings")]
    [Range(1, 100)]
    [SerializeField] private int maxSlots = 30;

    [Header("Items")]
    public List<InventoryItem> items;

    public event Action onItemChanged;

    private void Awake()
    {
        Instance = this;
    }
     private void Start()
    {
        InventoryManager.Instance.LoadInventoryData();
    }

    public void LoadItemsFromResources()
    {
        InventoryItem[] loadedItems = Resources.LoadAll<InventoryItem>("InventoryItems");

        items.Clear();

        items.AddRange(loadedItems);

        onItemChanged?.Invoke();
    }
    public void AddItem(InventoryItem item, int countToAdd = 0)
{
    if (items.Count < maxSlots)
    {
        items.Add(item);

        if (item.CanStack())
        {
            if (item is InventoryAmmo)
            {   
                InventoryAmmo ammoItem = (InventoryAmmo)item;

                if (ammoItem.ammoType == AmmoType.Pistol)
                {
                    ConsumableManager.Instance.pistolAmmoCount += countToAdd;
                }
                else if (ammoItem.ammoType ==  AmmoType.Riffle)
                {
                    ConsumableManager.Instance.akAmmoCount += countToAdd;
                }
            }
            else if (item is InventoryMedkit)
            {
                ConsumableManager.Instance.medkitCount += countToAdd;
            }
        }

        InventoryManager.Instance.SaveInventoryData();
        onItemChanged?.Invoke();
    }
}

public void RemoveItem(InventoryItem item, int countToRemove = 0)
{
    if (items.Contains(item))
    {
        items.Remove(item);

        if (item.CanStack())
        {
            if (item is InventoryAmmo)
            {
                InventoryAmmo ammoItem = (InventoryAmmo)item;

                if (ammoItem.ammoType ==  AmmoType.Pistol)
                {
                    ConsumableManager.Instance.pistolAmmoCount -= countToRemove;
                }
                else if (ammoItem.ammoType ==  AmmoType.Riffle)
                {
                    ConsumableManager.Instance.akAmmoCount -= countToRemove;
                }
            }
            else if (item is InventoryMedkit)
            {
                ConsumableManager.Instance.medkitCount -= countToRemove;
            }
        }

        InventoryManager.Instance.SaveInventoryData();
        onItemChanged?.Invoke();
    }
}


}
