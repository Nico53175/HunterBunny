using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int itemId;
    public Sprite itemSprite;
    public Mesh itemMesh;
    public Material itemMaterial; 
}
