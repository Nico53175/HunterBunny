using System;
using System.Diagnostics;
using UnityEngine;

public class InventorySystem 
{
    int xSize;
    int ySize;
    int[,] inventory;
    public InventorySystem(int xSize, int ySize)
    {
        this.xSize = xSize;
        this.ySize = ySize;
        inventory = new int[this.xSize, this.ySize];
    }   

    public void AddItem(int itemId)
    {
        int[] indeces;
        indeces = FindNextSpot(inventory,0);
        inventory[indeces[0], indeces[1]] = itemId;
    }

    private int[] FindNextSpot(int[,] inventory, int itemId)
    {
        int[] indeces = new int[2];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                if (inventory[x, y] == itemId)
                {
                    indeces[0] = x;
                    indeces[1] = y;
                    return indeces;
                }
            }
           
        }
        return null;
    }
}
