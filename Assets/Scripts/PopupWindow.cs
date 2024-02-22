using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button actionButton1;
    [SerializeField] private Button actionButton2;

    private InventoryItem currentItem;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        UpdateItemInfo(item);
        SetupActionButtons(item);
    }

    private void UpdateItemInfo(InventoryItem item)
    {
        itemNameText.text = item.itemName;
        itemWeightText.text = item.weight.ToString();

        if (item.icon != null)
        {
            itemImage.sprite = item.icon;
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            itemImage.gameObject.SetActive(false);
        }

        UpdateItemTypeText(item);
    }

    private void UpdateItemTypeText(InventoryItem item)
    {
        if (item is InventoryAmmo)
        {
            InventoryAmmo ammoItem = (InventoryAmmo)item;
            if (ammoItem.ammoType == AmmoType.Pistol)
            {
                itemTypeText.text = ConsumableManager.Instance.pistolAmmoCount.ToString();
            }
            else if (ammoItem.ammoType == AmmoType.Riffle)
            {
                itemTypeText.text = ConsumableManager.Instance.akAmmoCount.ToString();
            }
            }
            else if (item is InventoryMedkit)
            {
                itemTypeText.text = ConsumableManager.Instance.medkitCount.ToString();
            }
            else if (item is InventoryEquipment)
                {
                    InventoryEquipment equipmentItem = (InventoryEquipment)item;
                    if (equipmentItem.equipmentType == EquipmentType.Clothing || equipmentItem.equipmentType == EquipmentType.Helmet)
                    {
                        itemTypeText.text = "" + equipmentItem.protection.ToString();
                    }
                }
        
    }


    private void SetupActionButtons(InventoryItem item)
    {
        actionButton1.onClick.RemoveAllListeners();
        actionButton2.onClick.RemoveAllListeners();

        if (item is InventoryAmmo)
        {
            InventoryAmmo ammoItem = (InventoryAmmo)item;
            if (ammoItem.ammoType == AmmoType.Pistol)
            {
                SetupAmmoPistolActions();
            }
            else if (ammoItem.ammoType == AmmoType.Riffle)
            {
                SetupAmmoAKActions();
            }
        
        }
        else if (item is InventoryMedkit)
        {
            SetupMedkitActions();
        }
        else if (item is InventoryEquipment)
        {
            InventoryEquipment equipmentItem = (InventoryEquipment)item;
            if (equipmentItem.equipmentType == EquipmentType.Clothing)
            {
                SetupClothingActions();
            }
            else if (equipmentItem.equipmentType == EquipmentType.Helmet)
            {
                SetupHelmetActions();
            }
        }
    }

    private void SetupAmmoPistolActions()
    {
        actionButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Купить";
        actionButton1.onClick.AddListener(() => BuyAmmo(AmmoType.Pistol));
        actionButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Удалить";
        actionButton2.onClick.AddListener(DeleteItem);
    }


    private void SetupAmmoAKActions()
    {
        actionButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Купить";
        actionButton1.onClick.AddListener(() => BuyAmmo(AmmoType.Riffle));
        actionButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Удалить";
        actionButton2.onClick.AddListener(DeleteItem);
    }

    private void SetupMedkitActions()
    {
        actionButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Лечить";
        actionButton1.onClick.AddListener(HealPlayer);
        actionButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Удалить";
        actionButton2.onClick.AddListener(DeleteItem);
    }

    private void SetupClothingActions()
    {
        actionButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Экипировать";
        actionButton1.onClick.AddListener(EquipClothing);
        actionButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Удалить";
        actionButton2.onClick.AddListener(DeleteItem);
    }

    private void SetupHelmetActions()
    {
        actionButton1.GetComponentInChildren<TextMeshProUGUI>().text = "Экипировать";
        actionButton1.onClick.AddListener(EquipHelmet);
        actionButton2.GetComponentInChildren<TextMeshProUGUI>().text = "Удалить";
        actionButton2.onClick.AddListener(DeleteItem);
    }
   
    private void BuyAmmo(AmmoType ammoType)
{
    if (currentItem is InventoryAmmo)
    {
        InventoryAmmo ammoItem = (InventoryAmmo)currentItem;
        int remainingSpace = 0;
        int currentAmmoCount = 0;

        switch (ammoType)
        {
            case AmmoType.Pistol:
                remainingSpace = ammoItem.stackSize - ConsumableManager.Instance.pistolAmmoCount;
                currentAmmoCount = ConsumableManager.Instance.pistolAmmoCount;
                break;
            case AmmoType.Riffle:
                remainingSpace = ammoItem.stackSize - ConsumableManager.Instance.akAmmoCount;
                currentAmmoCount = ConsumableManager.Instance.akAmmoCount;
                break;
            default:
                Debug.LogError("Unsupported ammo type: " + ammoType);
                return;
        }

        if (remainingSpace > 0)
        {
            int numToAdd = Mathf.Min(remainingSpace, ammoItem.stackSize);
            currentAmmoCount += numToAdd;

            switch (ammoType)
            {
                case AmmoType.Pistol:
                    ConsumableManager.Instance.pistolAmmoCount = currentAmmoCount;
                    break;
                case AmmoType.Riffle:
                    ConsumableManager.Instance.akAmmoCount = currentAmmoCount;
                    break;
            }

            Debug.Log("Куплено " + numToAdd + " патронов. Текущее количество: " + currentAmmoCount);
            WeaponSwitch.Instance.UpdateAmmoCount();
        }
        else
        {
            int numToAdd = Mathf.Min(remainingSpace, ammoItem.stackSize);
            Inventory.Instance.AddItem(ammoItem, numToAdd);
            Debug.Log("Текущий стек патронов заполнен. Добавлен новый стек патронов");
        }
        
        InventoryManager.Instance.SaveInventoryData();
        float totalWeight = currentAmmoCount * ammoItem.weightPerAmmo;
        currentItem.weight = totalWeight;
        UpdateInventoryUI();
        itemWeightText.text = totalWeight.ToString();
        gameObject.SetActive(false);
    }
}

    private void HealPlayer()
    {
        if (currentItem is InventoryMedkit)
        {
            InventoryMedkit medkitItem = (InventoryMedkit)currentItem;
            
            if (ConsumableManager.Instance.medkitCount > 0)
            {
                Debug.Log("Игрок вылечен на " + medkitItem.hpRestore + " HP");

                ConsumableManager.Instance.medkitCount--;
                PlayerHealth.Instance.Heal(medkitItem.hpRestore);
                InventoryManager.Instance.SaveInventoryData();
                UpdateInventoryUI();
                
                if (ConsumableManager.Instance.medkitCount == 0)
                {
                    DeleteItem();
                }
                
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Нет доступных аптечек для использования!");
            }
        }
    }

    private void EquipClothing()
    {
        if (currentItem is InventoryEquipment)
        {
            EquipmentUI.Instance.EquipItem(currentItem);
            Debug.Log("Одежда успешно экипирована");
            gameObject.SetActive(false);
            DeleteItem();
            currentItem = null; 
            
        }
    }

    private void EquipHelmet()
    {
        if (currentItem is InventoryEquipment)
        {
            EquipmentUI.Instance.EquipItem(currentItem);
            DeleteItem();
            Debug.Log("Одежда успешно экипирована");
            gameObject.SetActive(false);
            currentItem = null;
        
        }
    }

    private void DeleteItem()
    {
        if (currentItem != null)
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            if (inventory != null)
            {
                inventory.RemoveItem(currentItem);
                Debug.Log("Предмет " + currentItem.itemName + " удален из инвентаря");
                gameObject.SetActive(false);
                UpdateInventoryUI();
            }
            else
            {
                Debug.LogWarning("Инвентарь не найден");
            }
        }

    }

    private void UpdateInventoryUI()
    {
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.UpdateUI();
        }
        else
        {
            Debug.LogWarning("Инвентарь UI не найден");
        }
    }

}
