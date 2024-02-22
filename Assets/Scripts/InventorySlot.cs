using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI stackText;
    [SerializeField] private PopupWindow popupWindowPrefab;

    private InventoryItem currentItem;

    private void Awake()
    {
        iconImage = GetComponentInChildren<Image>();
        stackText = GetComponentInChildren<TextMeshProUGUI>();
    }

        public void SetItem(InventoryItem item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;

        if (item.CanStack())
        {
            int stackCount = 0;
        
            if (item is InventoryAmmo)
            {
                InventoryAmmo ammoItem = (InventoryAmmo)item;
                if (ammoItem.ammoType == AmmoType.Pistol)
                {
                    stackCount = ConsumableManager.Instance.pistolAmmoCount;
                }
                else if (ammoItem.ammoType == AmmoType.Riffle)
                {
                    stackCount = ConsumableManager.Instance.akAmmoCount;
                }
            }
            else if (item is InventoryMedkit)
            {
                stackCount = ConsumableManager.Instance.medkitCount;
            }
            else
            {
                stackCount = item.stackSize;
            }

            if (stackCount > 1)
            {
                stackText.text = "x" + stackCount.ToString();
            }
            else
            {
                stackText.text = "";
            } 
        }
        else
        {
            stackText.text = "";
        }

    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenPopupWindow();
    }

    private void OpenPopupWindow()
    {
        if (currentItem != null)
        {
            PopupWindow popupWindow = Instantiate(popupWindowPrefab);
            popupWindow.SetItem(currentItem);
        }
    }
}
