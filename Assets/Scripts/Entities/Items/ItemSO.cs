using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemSprite;
    public Mesh itemMesh;
    public Material itemMaterial; 
}
