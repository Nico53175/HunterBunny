using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Food,
        Mineral,
        Tool
    }

    public string itemName;
    public int itemId;
    public Sprite itemSprite;
    public Mesh itemMesh;
    public Material itemMaterial;    
    public ItemType itemType;
}
