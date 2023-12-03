using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private InventoryData inventoryData;
    [SerializeField] private int xSizeMax;
    [SerializeField] private int ySizeMax;
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    
    private InventoryCell[,] inventoryCells;
    [SerializeField] private InventoryCell cellPrefab;
    [SerializeField] private Transform cellParent;

    private InventoryCraftingCell[] craftingCells;
    [SerializeField] private InventoryCraftingCell craftingCellPrefab;
    [SerializeField] private Transform craftingCellParent;

    [SerializeField] private ItemDatasetSO itemDataSet;
    private Dictionary<int, ItemSO> itemLookupTable;

    float mouseScroll;
    [SerializeField] Color selectedItemBorderColor;
    [SerializeField] private Color itemBorderColor;

    private void Start()
    {
        itemLookupTable = new Dictionary<int, ItemSO>();
        foreach (var item in itemDataSet.items)
        {
            itemLookupTable[item.itemId] = item;
        }

        inventoryData = new InventoryData(xSize, ySize);
        inventoryCells = new InventoryCell[xSize, ySize];
        InitializeUI();

        inventoryData.AddItem(1, 8);
        inventoryData.AddItem(2, 8);
        RefreshChangedInventoryUI();

        Canvas canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void Update()
    {
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        UpdateSelectedCell();
    }

    // Mouse Scroll Behavior
    private void UpdateSelectedCell()
    {
        if (mouseScroll > 0 || mouseScroll < 0)
        {
            int oldSelectedCellIndex = 0;
            int newSelectedCellIndex = 0;
            for (int i = 0; i < xSize; i++)
            {
                if (inventoryCells[i, 0].IsSelected())
                {
                    oldSelectedCellIndex = i;
                    break;
                }
            }

            if (mouseScroll > 0) // Scroll up
            {
                newSelectedCellIndex = (oldSelectedCellIndex + 1) % xSize;
            }
            else // Scroll down
            {
                newSelectedCellIndex = (oldSelectedCellIndex - 1 + xSize) % xSize;
            }

            InventoryCell newSelectedCell = inventoryCells[newSelectedCellIndex, 0];
            newSelectedCell.SetSelected(true);
            newSelectedCell.backgroundImageComponent.color = selectedItemBorderColor;

            InventoryCell oldSelectedCell = inventoryCells[oldSelectedCellIndex, 0];
            oldSelectedCell.SetSelected(false);
            oldSelectedCell.backgroundImageComponent.color = itemBorderColor;            
        }
    }

    // Initialize Methodes
    private void InitializeUI()
    {
        InitializeInventoryCells();
        InitializeCraftingCells();
    }

    private void InitializeCraftingCells()
    {
        Debug.Log("Initializing");
        List<ItemSO> craftableItemsSO = new List<ItemSO>();
        foreach(KeyValuePair<int, ItemSO> kvp in itemLookupTable)
        {
            if(kvp.Value.craftingIngredients.Count > 0)
            {
                craftableItemsSO.Add(kvp.Value);
            }
        }

        if(craftableItemsSO.Count > 0)
        {
            craftingCells = new InventoryCraftingCell[craftableItemsSO.Count];

            for (int i = 0; i < craftingCells.Length; i++)
            {
                Debug.Log("New Crafting Cell");

                // Calculate Position is missing
                ItemSO item = craftableItemsSO[i];
                InventoryCraftingCell cell = Instantiate(craftingCellPrefab, craftingCellParent);
                cell.GetComponent<Image>().sprite = item.itemSprite;
                cell.Initialize(this, item.itemId, item.craftingIngredients);                
                craftingCells[i] = cell;

                RectTransform rectTransform = cell.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 2);
                rectTransform.anchorMax = new Vector2(0, 2);
                rectTransform.pivot = new Vector2(0, 1);
                cell.gameObject.SetActive(false);
            }
        }
    }

    private void InitializeInventoryCells()
    {
        GridLayoutGroup cellLayoutGroup = cellParent.GetComponent<GridLayoutGroup>();
        cellLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        cellLayoutGroup.constraintCount = xSize;

        for (int y = 0; y < ySize; y++) // Y first, because the LayoutGroup goes row by row and not column by column
        {
            for (int x = 0; x < xSize; x++)
            {
                InventoryCell cell = Instantiate(cellPrefab, cellParent);
                cell.GetComponent<Image>().sprite = null;
                cell.Initialize(this, x, y);
                cell.itemImageComponent.enabled = false;
                inventoryCells[x, y] = cell;
            }
        }

        inventoryCells[0, 0].SetSelected(true);
    }


    // Crafting Methodes
    public void FindCraftableItems()
    {
        foreach(InventoryCraftingCell cell in craftingCells)
        {
            bool canBeCrafted = true;

            // Check if all ingredients are present in the required quantity
            foreach (var ingredient in cell.craftingIngredients)
            {
                int requiredQuantity = ingredient.count;
                int ingredientId = ingredient.itemId;
                int availableQuantity = inventoryData.GetCountById(ingredientId);

                if (availableQuantity < requiredQuantity)
                {
                    canBeCrafted = false;
                    break;
                }
            }
            cell.gameObject.SetActive(canBeCrafted);            
        }
    }



    // Refresh Methodes
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
                        cell.itemImageComponent.sprite = item.itemSprite;
                        cell.textComponent.text = inventory[x, y].itemCount.ToString();
                    }
                }
                else
                {
                    cell.itemImageComponent.sprite = null;
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
                cell.itemImageComponent.enabled = true;
                cell.itemImageComponent.sprite = item?.itemSprite;
                cell.textComponent.text = itemData.itemCount.ToString();
            }
            else
            {
                cell.itemImageComponent.sprite = null;
                cell.itemImageComponent.enabled = false;
                cell.textComponent.text = "";
            }
        }

        inventoryData.ClearChangedCells();
    }

    // Inventory Methodes
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

    public void AddItem(int itemId, int itemCount)
    {
        inventoryData.AddItem(itemId, itemCount);
        RefreshChangedInventoryUI();
    }

    public void DeleteItem(int itemId) // Remove Item
    {
        Vector2? index = inventoryData.GetIndexById(itemId);
        if (index.HasValue) 
        {
            int xIndex = (int)index.Value.x;
            int yIndex = (int)index.Value.y;

            inventoryData.DeleteItem(itemId);
            RefreshChangedInventoryUI();
        }
    }

    public void DeleteItem(int itemId, int count) // Remove an ammount of Items
    {
        Vector2? index = inventoryData.GetIndexById(itemId);
        if (index.HasValue)
        {
            int xIndex = (int)index.Value.x;
            int yIndex = (int)index.Value.y;

            inventoryData.DeleteItem(itemId, count);
            
            RefreshChangedInventoryUI();
        }
    }

    // Event Listeners
    private void OnEnable()
    {
        playerController.OnItemPickedUp += AddItem;
        playerController.OnCraftingTableOpened += FindCraftableItems;
    }

    private void OnDisable()
    {
        playerController.OnItemPickedUp -= AddItem;
        playerController.OnCraftingTableOpened -= FindCraftableItems;
    }
}
