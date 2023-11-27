using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryCraftingCell : MonoBehaviour
{
    private InventoryManager inventoryManager;

    [SerializeField] public Image imageComponent;
    [HideInInspector] public Vector2 index;

    public void Initialize(InventoryManager inventoryManager, int x, int y)
    {
        this.inventoryManager = inventoryManager;
        index = new Vector2(x, y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // Get One Item per Click
        {                
            
        }
    }
}
