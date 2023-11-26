using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
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
    private Dictionary<int, ItemSO> itemLookup;

    private void Start()
    {
        itemLookup = new Dictionary<int, ItemSO>();
        foreach (var item in lookUpTable.items)
        {
            itemLookup[item.itemId] = item;
        }

        inventoryData = new InventoryData(xSize, ySize);
        inventoryCells = new InventoryCell[xSize, ySize];
        InitializeUI();
    }

    private void InitializeUI()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Vector2 pos = new Vector2(x * 100 + 10, y * -100 - 10);
                InventoryCell cell = Instantiate(cellPrefab, pos, Quaternion.identity, cellParent);
                cell.GetComponent<Image>().sprite = defaultSprite;
                cell.Initialize(this, x, y);
                inventoryCells[x, y] = cell;
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
        if (itemLookup.TryGetValue(id, out ItemSO item))
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

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButtonDown(0) && other.tag == "Item")
        {
            ItemStructure item = other.GetComponent<ItemStructure>();
            int itemId = item.GetItemId();
            int itemCount = item.GetItemCount();
            inventoryData.AddItem(itemId, itemCount);
            RefreshChangedInventoryUI();
        }
    }
}
