using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// Place on objects that can drop inventory items.
    /// Handles tracking of drops for saving purposes.
    /// </summary>
public class ItemDropper : MonoBehaviour
{
    private List<Pickup> droppedItems = new List<Pickup>();

    public void DropItem(InventoryItem item, int number){
        SpawnPickup(item, GetDropLocation(), number);
    }

    protected virtual Vector3 GetDropLocation(){
        return transform.position; // change here if the drop location needs to differ
    }

    public void SpawnPickup(InventoryItem item, Vector3 location, int number){
        Pickup pickup = item.SpawnPickup(location, number);
        pickup.GetComponent<PickupUI>().Setup();
        droppedItems.Add(pickup);
    }

    /// <summary>
    /// Remove any drops in the world that have subsequently been picked up.
    /// </summary>
    private void RemoveDestroyedDrops(){
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
    }
}
