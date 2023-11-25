using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStructure : MonoBehaviour
{
    [SerializeField] ItemSO itemData; 

    int id;
    string name;
    Sprite itemSprite;
    Mesh itemMesh;
    Material itemMaterial;
    // Start is called before the first frame update
    void Start()
    {
        id = itemData.itemID; 
        name = itemData.name;
        itemSprite = itemData.itemSprite;
        itemMaterial = itemData.itemMaterial;
        itemMesh = itemData.itemMesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshRenderer.material = itemMaterial;
        meshFilter.mesh = itemMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
