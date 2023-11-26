using UnityEngine;

public class ItemStructure : MonoBehaviour
{
    [SerializeField] ItemSO itemData; 

    int itemID;
    string itemName;
    Sprite itemSprite;
    Mesh itemMesh;
    Material itemMaterial;
    // Start is called before the first frame update
    void Start()
    {
        itemID = itemData.itemId; 
        name = itemData.name;
        itemSprite = itemData.itemSprite;
        itemMaterial = itemData.itemMaterial;
        itemMesh = itemData.itemMesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshRenderer.material = itemMaterial;
        meshFilter.mesh = itemMesh;
    }

    public int GetItemId()
    {
        return itemID;
    }
}
