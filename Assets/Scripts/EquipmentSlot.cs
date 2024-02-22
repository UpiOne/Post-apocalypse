using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    [Header("Equipment Settings")]
    [SerializeField] public EquipSlot helmetSlot;
    [SerializeField] public EquipSlot armorSlot;

    public void EquipItem(InventoryItem item)
    {
        if (item is InventoryEquipment)
        {
            InventoryEquipment equipmentItem = (InventoryEquipment)item;
            if (equipmentItem.equipmentType == EquipmentType.Helmet)
            {
                EquipHelmet(equipmentItem);
            }
            else if (equipmentItem.equipmentType == EquipmentType.Clothing)
            {
                EquipArmor(equipmentItem);
            }

            InventoryManager.Instance.SaveInventoryData();
        }
    }

    private void EquipHelmet(InventoryEquipment helmet)
    {
        PlayerHealth.Instance.armor += helmet.protection;
        if (helmetSlot != null)
        {
            if (helmetSlot.currentItem != null)
            {
                InventoryItem currentItem = helmetSlot.currentItem;
                helmetSlot.SetItem(helmet);
                FindObjectOfType<Inventory>().AddItem(currentItem);
            }
            else
            {
                helmetSlot.SetItem(helmet);
            }
        }
        else
        {
            Debug.LogWarning("Helmet slot is not assigned!");
        }
    }

    private void EquipArmor(InventoryEquipment armor)
    {
        PlayerHealth.Instance.armor += armor.protection;
        if (armorSlot != null)
        {
            if (armorSlot.currentItem != null)
            {
                InventoryItem currentItem = armorSlot.currentItem;
                armorSlot.SetItem(armor);
                FindObjectOfType<Inventory>().AddItem(currentItem);
            }
            else
            {
                armorSlot.SetItem(armor);
            }
        }
        else
        {
            Debug.LogWarning("Armor slot is not assigned!");
        }
    }
}
