using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour
{
    [Header("Slot Settings")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemStats;

    public InventoryEquipment currentItem; 

    public void SetItem(InventoryEquipment item) 
    {
        currentItem = item;
        iconImage.sprite = item.icon; 
        
        if (itemStats != null)
        {
            itemStats.text = item.itemName;
        }

        if (item.equipmentType == EquipmentType.Clothing)
        {
            itemStats.text = item.protection.ToString();
        }
        else if (item.equipmentType == EquipmentType.Helmet)
        {
            itemStats.text = item.protection.ToString();
        }
        else
        {
            itemStats.text = "";
        }
    }
}
