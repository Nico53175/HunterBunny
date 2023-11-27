using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private InventoryData inventoryData;
    private InventoryCell[,] inventoryCells;
    [SerializeField] private int xSizeMax;
    [SerializeField] private int ySizeMax;
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    [SerializeField] InventoryCell cellPrefab;
    [SerializeField] Transform cellParent;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] ItemDatasetSO lookUpTable;
    private Dictionary<int, ItemSO> itemLookupTable;

    private void Start()
    {
        itemLookupTable = new Dictionary<int, ItemSO>();
        foreach (var item in lookUpTable.items)
        {
            itemLookupTable[item.itemId] = item;
        }

        inventoryData = new InventoryData(xSize, ySize);
        inventoryCells = new InventoryCell[xSize, ySize];
        InitializeUI();

        inventoryData.AddItem(itemLookupTable[1].itemId, 2);
        inventoryData.AddItem(itemLookupTable[2].itemId, 5);
        RefreshChangedInventoryUI();

        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void InitializeUI()
    {
        RectTransform prefabRect = cellPrefab.GetComponent<RectTransform>();
        float cellWidth = prefabRect.sizeDelta.x; // Width of each cell
        float cellHeight = prefabRect.sizeDelta.y; // Height of each cell
        float spacing = 50; // Spacing between cells

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                // Calculate position for each cell
                float posX = (x * (cellWidth + spacing)) + cellWidth / 2;
                float posY = -((y * (cellHeight + spacing)) + cellHeight / 2);

                Vector2 pos = new Vector2(posX, posY);

                InventoryCell cell = Instantiate(cellPrefab, pos, Quaternion.identity, cellParent);
                cell.GetComponent<Image>().sprite = defaultSprite;
                cell.Initialize(this);
                inventoryCells[x, y] = cell;

                // Set the anchor to the top left
                RectTransform rectTransform = cell.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 2);
                rectTransform.anchorMax = new Vector2(0, 2);
                rectTransform.pivot = new Vector2(0, 1);
            }
        }
    }


    private void RefreshAllInventoryUI()
    {
        InventoryData.ItemData[,] inventory = inventoryData.GetInventoryData();
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                InventoryCell cell = inventoryCells[x, y];
                if (inventory[x, y].itemId != 0)
                {
                    ItemSO item = FindItemSOById(inventory[x, y].itemId);
                    if (item != null)
                    {
                        cell.imageComponent.sprite = item.itemSprite;
                        cell.textComponent.text = inventory[x, y].itemCount.ToString();
                    }
                }
                else
                {
                    cell.imageComponent.sprite = defaultSprite;
                    cell.textComponent.text = "";
                }
            }
        }
    }

    private void RefreshChangedInventoryUI()
    {
        foreach (Vector2 cellIndex in inventoryData.ChangedCells)
        {
            int x = (int)cellIndex.x;
            int y = (int)cellIndex.y;
            InventoryCell cell = inventoryCells[x, y];
            InventoryData.ItemData itemData = inventoryData.GetInventoryData()[x, y];

            if (itemData.itemId != 0)
            {
                ItemSO item = FindItemSOById(itemData.itemId);
                cell.imageComponent.sprite = item?.itemSprite;
                cell.textComponent.text = itemData.itemCount.ToString();
            }
            else
            {
                cell.imageComponent.sprite = defaultSprite;
                cell.textComponent.text = "";
            }
        }

        inventoryData.ClearChangedCells();
    }

    private ItemSO FindItemSOById(int id)
    {
        if (itemLookupTable.TryGetValue(id, out ItemSO item))
        {
            return item;
        }
        return null;
    }

    public void SwapItems(InventoryCell sourceCell, InventoryCell targetCell)
    {
        inventoryData.SwapData(sourceCell.index, targetCell.index);
        RefreshChangedInventoryUI();
    }

    private void AddItem(int itemId, int itemCount)
    {
        inventoryData.AddItem(itemId, itemCount);
        RefreshChangedInventoryUI();
    }

    public void DeleteItem(int itemId)
    {
        Vector2? index = inventoryData.GetIndexById(itemId);
        if (index.HasValue) 
        {
            int xIndex = (int)index.Value.x;
            int yIndex = (int)index.Value.y;

            inventoryData.RemoveItem(itemId);
            RefreshChangedInventoryUI();
        }
    }

    private void OnEnable()
    {
        // Subscribe to the event
        playerController.OnItemPickedUp += AddItem;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        playerController.OnItemPickedUp -= AddItem;
    }
}
