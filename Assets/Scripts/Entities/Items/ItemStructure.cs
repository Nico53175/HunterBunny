using System;
using UnityEngine;

public class ItemStructure : MonoBehaviour
{
    [SerializeField] ItemSO itemData; 

    int itemID;
    int itemCount = 1;
    string itemName;
    Sprite itemSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        itemID = itemData.itemId; 
        name = itemData.name;
        itemSprite = itemData.itemSprite;
    }

    public int GetItemId()
    {
        return itemID;
    }

    internal int GetItemCount()
    {
        return itemCount;
    }
}
