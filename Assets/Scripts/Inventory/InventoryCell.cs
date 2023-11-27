using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private Transform originalParent;
    private InventoryManager inventoryManager;
    [HideInInspector] public Vector2 index;
    [SerializeField] public Image imageComponent;
    [SerializeField] public TMP_Text textComponent;
    private Vector2 centerOffset;
    public void Initialize(InventoryManager manager, int x, int y)
    {
        inventoryManager = manager;
        index = new Vector2(x, y);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
        centerOffset = (Vector2)transform.position - eventData.position;
        transform.SetAsFirstSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 newPosition = eventData.position + centerOffset;
        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = originalPosition;
        transform.SetParent(originalParent);
        GameObject target = eventData.pointerCurrentRaycast.gameObject;

        // Check if dropped on a valid target
        if (target != null && target.tag == "Inventory Cell")
        {
            InventoryCell targetCell = target.GetComponent<InventoryCell>();
            if (targetCell != null)
            {
                inventoryManager.SwapItems(this, targetCell);
            }
        }
    }
}
