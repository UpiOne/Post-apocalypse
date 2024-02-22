using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SavedInventoryData
{
    public InventoryItem[] items;
    public InventoryItem equippedHelmet;
    public InventoryItem equippedArmor;
    public int pistolAmmoCount;
    public int akAmmoCount;
    public int medkitCount;

    public SavedInventoryData(InventoryItem[] items, InventoryItem equippedHelmet, InventoryItem equippedArmor, int pistolAmmoCount, int akAmmoCount, int medkitCount)
    {
        this.items = items;
        this.equippedHelmet = equippedHelmet;
        this.equippedArmor = equippedArmor;
        this.pistolAmmoCount = pistolAmmoCount;
        this.akAmmoCount = akAmmoCount;
        this.medkitCount = medkitCount;
    }
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SaveInventoryData()
    {
        SavedInventoryData data = new SavedInventoryData(Inventory.Instance.items.ToArray(), EquipmentUI.Instance.equipmentSlot.helmetSlot.currentItem, EquipmentUI.Instance.equipmentSlot.armorSlot.currentItem, ConsumableManager.Instance.pistolAmmoCount, ConsumableManager.Instance.akAmmoCount, ConsumableManager.Instance.medkitCount);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/inventoryData.json", json);
    }

    public void LoadInventoryData()
    {
        string path = Application.persistentDataPath + "/inventoryData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedInventoryData data = JsonUtility.FromJson<SavedInventoryData>(json);

            // Populate inventory with saved data
            Inventory.Instance.items.Clear();
            foreach (InventoryItem item in data.items)
            {
                Inventory.Instance.AddItem(item);
            }

            // Equip saved items
            EquipmentUI.Instance.equipmentSlot.EquipItem(data.equippedHelmet);
            EquipmentUI.Instance.equipmentSlot.EquipItem(data.equippedArmor);

            // Update consumable counts
            ConsumableManager.Instance.pistolAmmoCount = data.pistolAmmoCount;
            ConsumableManager.Instance.akAmmoCount = data.akAmmoCount;
            ConsumableManager.Instance.medkitCount = data.medkitCount;
        }
    }
}
