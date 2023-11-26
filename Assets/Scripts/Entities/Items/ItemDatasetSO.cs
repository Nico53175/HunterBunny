using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataset", menuName = "Item/Item Dataset", order = 2)]
public class ItemDatasetSO : ScriptableObject
{
    public ItemSO[] items;
}
