using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingIngredient
{
    public int itemId;
    public int count;
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Food,
        Mineral,
        Tool,
        Weapon
    }

    public string itemName;
    public int itemId;
    public Sprite itemSprite;    
    public ItemType itemType;
    public List<CraftingIngredient> craftingIngredients;
}
