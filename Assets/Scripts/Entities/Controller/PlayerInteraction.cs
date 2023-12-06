using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public delegate void WorkBenchOpenedEventHandler();
    public event WorkBenchOpenedEventHandler OnCraftingTableOpened;

    public delegate void ItemPickedUpEventHandler(int itemId, int itemCount);
    public event ItemPickedUpEventHandler OnItemPickedUp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log(other.gameObject.name);

            ItemStructure item;
            int itemId;
            int itemCount;
            Destroy(other.gameObject);

            if (other.transform.parent != null)
            {
                GameObject parent = other.transform.parent.gameObject;
                item = parent.GetComponent<ItemStructure>();
                itemId = item.GetItemId();
                itemCount = item.GetItemCount();
                Destroy(parent);
                OnItemPickedUp?.Invoke(itemId, itemCount);
            }
            else
            {
                item = other.GetComponent<ItemStructure>();
                itemId = item.GetItemId();
                itemCount = item.GetItemCount();
                Destroy(other.gameObject);
                OnItemPickedUp?.Invoke(itemId, itemCount);
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Crafting Bench"))
        {
            OnCraftingTableOpened?.Invoke();
        }
    }
}
