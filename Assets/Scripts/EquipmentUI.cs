using UnityEngine;

public class EquipmentUI : MonoBehaviour
{   
    [Header("UI Settings")]
    public EquipmentSlot equipmentSlot;
    public static EquipmentUI Instance;

    void Awake()
    {
        Instance = this;
    } 

    public void EquipItem(InventoryItem item)
    {
        if (equipmentSlot == null)
        {
            Debug.LogError("Equipment slot is not assigned!");
            return;
        }

        if (item is InventoryEquipment)
        {
            InventoryEquipment equipmentItem = (InventoryEquipment)item;
            if (equipmentItem.equipmentType == EquipmentType.Helmet || equipmentItem.equipmentType == EquipmentType.Clothing)
            {
                equipmentSlot.EquipItem(item);
                Inventory.Instance.RemoveItem(item);
                InventoryManager.Instance.SaveInventoryData();
            }
            else
            {
                Debug.LogWarning("Unsupported equipment type: " + equipmentItem.equipmentType);
            }
        }
        else
        {
            Debug.LogWarning("Unsupported item type for equipment: " + item.itemType);
        }
    }  
}
