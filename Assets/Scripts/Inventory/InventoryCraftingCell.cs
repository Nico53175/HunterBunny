using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCraftingCell : MonoBehaviour, IPointerClickHandler
{
    private InventoryManager inventoryManager;

    [SerializeField] public Image imageComponent;
    [HideInInspector] public int itemId;
    [HideInInspector] public List<CraftingIngredient> craftingIngredients;

    public void Initialize(InventoryManager inventoryManager, int itemId, List<CraftingIngredient> craftingIngredients)
    {
        this.inventoryManager = inventoryManager;
        this.itemId = itemId;
        this.craftingIngredients = craftingIngredients;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // Get One Item per Click
        {
            foreach(var item in craftingIngredients)
            {
                inventoryManager.DeleteItem(item.itemId, item.count);
            }
            inventoryManager.AddItem(itemId, 1);
            inventoryManager.FindCraftableItems();
        }
    }
}
