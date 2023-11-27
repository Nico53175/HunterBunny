using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InnventoryCraftingCell : MonoBehaviour
{
    private Vector2 originalPosition;
    private Transform originalParent;
    private InventoryManager inventoryManager;

    [SerializeField] public Image imageComponent;
    [SerializeField] public TMP_Text textComponent;
    private Vector2 index;

    public void Initialize(InventoryManager manager, int x, int y)
    {
        inventoryManager = manager;
        index = new Vector2(x, y);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
