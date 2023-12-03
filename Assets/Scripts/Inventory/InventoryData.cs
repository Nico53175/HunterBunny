using System.Collections.Generic;
using UnityEngine;

public class InventoryData
{
    private int xSize;
    private int ySize;
    private ItemData[,] inventory;
    private List<Vector2> changedCells;
    public List<Vector2> ChangedCells => changedCells;
    public struct ItemData
    {
        public int itemId;
        public int itemCount;
    }

    public InventoryData(int xSize, int ySize)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        inventory = new ItemData[this.xSize, this.ySize];

        changedCells = new List<Vector2>();
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                inventory[x, y].itemId = 0;
                inventory[x, y].itemCount = 0;
            }
        }
    }   

    public ItemData[,] GetInventoryData()
    {
        return inventory;
    }

    public void DeleteItem(int itemId)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == itemId)
                {
                    inventory[x, y].itemId = 0;
                    inventory[x, y].itemCount = 0;
                    changedCells.Add(new Vector2(x, y));
                    return;
                }
            }
        }
    }

    public void DeleteItem(int itemId, int count)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == itemId)
                {
                    inventory[x, y].itemCount -= count;

                    if(inventory[x, y].itemCount <= 0)
                    {
                        DeleteItem(itemId);
                    }
                    else
                    {
                        changedCells.Add(new Vector2(x, y));
                    }
                }
            }
        }
    }

    public void AddItem(int itemId, int count)
    {
        bool itemExists;
        int[] index = FindItemById(inventory, itemId);
        itemExists = index != null;

        if (itemExists)  
        {
            inventory[index[0], index[1]].itemCount += count;
            changedCells.Add(new Vector2(index[0], index[1]));
        }
        else
        {
            if (!IsInvenntoryFull())
            {
                int[] freeSpotIndex = FindFreeSpot();
                if (freeSpotIndex != null) // Check if a free spot was found
                {
                    inventory[freeSpotIndex[0], freeSpotIndex[1]].itemId = itemId;
                    inventory[freeSpotIndex[0], freeSpotIndex[1]].itemCount = count;
                    changedCells.Add(new Vector2(freeSpotIndex[0], freeSpotIndex[1]));
                }
            }            
        }
    }

    private int[] FindItemById(ItemData[,] inventory, int itemId)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == itemId)
                {
                    return new int[] { x, y };
                }
            }           
        }
        return null;
    }

    private bool IsInvenntoryFull()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private int[] FindFreeSpot()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == 0) 
                {
                    return new int[] { x, y };
                }
            }
        }
        return null; 
    }

    public void SwapData(Vector2 sourceIndex, Vector2 targetIndex)
    {
        int sourceX = (int)sourceIndex.x;
        int sourceY = (int)sourceIndex.y;
        int targetX = (int)targetIndex.x;
        int targetY = (int)targetIndex.y;

        ItemData temp = inventory[sourceX, sourceY];
        inventory[sourceX, sourceY] = inventory[targetX, targetY];
        inventory[targetX, targetY] = temp;
        changedCells.Add(new Vector2(sourceX, sourceY));
        changedCells.Add(new Vector2(targetX, targetY));
    }

    public int GetCountByIndex(Vector2 index)
    {
        return inventory[(int)index.x, (int)index.y].itemCount;
    }

    public int GetIdByIndex(Vector2 index)
    {
        return inventory[(int)index.x, (int)index.y].itemId;
    }

    public int GetCountById(int itemId)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == itemId)
                {
                    return inventory[x, y].itemCount;
                }
            }
        }
        return 0;
    }

    public Vector2? GetIndexById(int itemId)
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y].itemId == itemId)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return null;
    }

    public void ClearChangedCells()
    {
        changedCells.Clear();
    }
}
