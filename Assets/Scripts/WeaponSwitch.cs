using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum WeaponType
{
    Pistol,
    Rifle
}

[System.Serializable]
public class Weapon
{
    public WeaponType type;
    public int damage;
    public int ammo;

    public Weapon(WeaponType type, int damage, int ammo)
    {
        this.type = type;
        this.damage = damage;
        this.ammo = ammo;
    }
}

public class WeaponSwitch : MonoBehaviour
{
    public static WeaponSwitch Instance;

     [Header("UI Elements")]
    [SerializeField] private Button pistolButton;
    [SerializeField] private Button rifleButton;
    [SerializeField] private Button shootButton;

    [Header("Sprites")]
    [SerializeField] private Sprite pistolImageSelected;
    [SerializeField] private Sprite rifleImageSelected;

    [Header("Weapons")]
    [SerializeField] private Weapon pistol;
    [SerializeField] private Weapon rifle;
    [SerializeField] private Weapon currentWeapon;


    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Inventory inventory;
    [SerializeField] private List<InventoryItem> dropItems;
     private InventoryItem currentItem;
    private bool isShootingToHead = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pistolButton.onClick.AddListener(SelectPistol);
        rifleButton.onClick.AddListener(SelectRifle);
        shootButton.onClick.AddListener(Shoot);

        SelectPistol();
        pistol.ammo = ConsumableManager.Instance.pistolAmmoCount;
        rifle.ammo = ConsumableManager.Instance.akAmmoCount;
    }

    private void SelectPistol()
    {
        currentWeapon = pistol;
        Debug.Log("Выбран пистолет");
        ChangeButtonImage(pistolButton, pistolImageSelected);
        ChangeButtonImage(rifleButton, null);
    }

    private void SelectRifle()
    {
        currentWeapon = rifle;
        Debug.Log("Выбран автомат");
        ChangeButtonImage(rifleButton, rifleImageSelected);
        ChangeButtonImage(pistolButton, null);
    }

    private void ChangeButtonImage(Button button, Sprite selectedSprite)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = selectedSprite;
        }
        else
        {
            Debug.LogWarning("Компонент Image не найден на кнопке: " + button.name);
        }
    }

    private void Shoot()
    {
        if (currentWeapon != null && currentWeapon.ammo > 0)
        {
            UseAmmo();

            DealDamage();

            UpdatePlayerDamage();

            CheckEnemyHealth();

            UpdateAmmoCount();

            UpdateInventoryUI();
        }
        else
        {
            Debug.Log("Нет патронов!");
        }
    }

    private void UseAmmo()
    {
        if (currentWeapon.type == WeaponType.Pistol && ConsumableManager.Instance.pistolAmmoCount > 0)
        {
            ConsumableManager.Instance.pistolAmmoCount--;
            currentWeapon.ammo--;
            InventoryManager.Instance.SaveInventoryData();
            if (ConsumableManager.Instance.pistolAmmoCount == 0)
            {
                RemoveAmmoFromInventory(AmmoType.Pistol);
            }
        }
        else if (currentWeapon.type == WeaponType.Rifle && ConsumableManager.Instance.akAmmoCount > 0)
        {
            ConsumableManager.Instance.akAmmoCount -= 3;
            currentWeapon.ammo -= 3;
            InventoryManager.Instance.SaveInventoryData();
            if (ConsumableManager.Instance.akAmmoCount <= 0)
            {
                RemoveAmmoFromInventory(AmmoType.Riffle);
            }
        }
    }

    private void DealDamage()
    {
        int damage = currentWeapon.type == WeaponType.Pistol ? pistol.damage : rifle.damage;
        enemyHealth.TakeDamage(damage);
    }

    private void UpdatePlayerDamage()
    {
        int playerDamage = 15;
        if (isShootingToHead)
        {
            playerDamage *= 2;
        }
        playerDamage -= playerHealth.armor;
        playerDamage = Mathf.Max(playerDamage, 0);
        playerHealth.TakeDamage(playerDamage);
        isShootingToHead = !isShootingToHead;
    }

    private void CheckEnemyHealth()
    {
        if (enemyHealth.currentHealth <= 0)
        {
            enemyHealth.Die();
            DropItem();
        }
    }

    public void UpdateAmmoCount()
    {
        if (currentWeapon.type == WeaponType.Pistol)
        {
            currentWeapon.ammo = ConsumableManager.Instance.pistolAmmoCount;
        }
        else if (currentWeapon.type == WeaponType.Rifle)
        {
            currentWeapon.ammo = ConsumableManager.Instance.akAmmoCount;
        }
    }
    private void RemoveAmmoFromInventory(AmmoType ammoType)
    {
        InventoryAmmo item = (InventoryAmmo)inventory.items.Find(i => {
            if (i is InventoryAmmo ammoItem)
            {
                return ammoItem.ammoType == ammoType;
            }
            return false;
        });

        if (item != null)
        {
        
            inventory.RemoveItem(item);
            Debug.Log("Удален предмет из инвентаря: " + item.itemName);
        }
        else
        {
            Debug.LogWarning("Предмет не найден в инвентаре: " + ammoType.ToString());
        }

        UpdateInventoryUI();
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

    private void DropItem()
    {
        if (dropItems.Count > 0)
        {
            int randomIndex = Random.Range(0, dropItems.Count);
            InventoryItem randomItem = dropItems[randomIndex];
            inventory.AddItem(randomItem);
        }
        else
        {
            Debug.LogWarning("Список предметов пуст!");
        }
    }
}
