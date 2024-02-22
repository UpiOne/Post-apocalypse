using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject slotPrefab; 
    [SerializeField] private Transform[] itemsParent; 

    private Inventory inventory;

    void Start()
    {
        inventory = Inventory.Instance; 
        inventory.onItemChanged += UpdateUI; 
        UpdateUI(); 
    }
   
    public void UpdateUI()
    {
        foreach (Transform parent in itemsParent)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

        int parentIndex = 0; 

        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, itemsParent[parentIndex]); 

            InventorySlot slot = slotGO.GetComponent<InventorySlot>(); 

            if (slot != null)
            {
                slot.SetItem(inventory.items[i]); 
            }

            parentIndex = (parentIndex + 1) % itemsParent.Length;
        }
    }
}
