using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipment : MonoBehaviour
{

    Dictionary<EquipmentLocation, EquipableItem> equippedItems = new Dictionary<EquipmentLocation, EquipableItem>();

    public event Action equipmentUpdated;

    ///<summary>
    ///Returns the item currently in the parameter location
    ///<summary>
    public EquipableItem GetItemInSlot(EquipmentLocation loc){
        if(!equippedItems.ContainsKey(loc)){
            return null;
        } else {
            return equippedItems[loc];
        }
    }

    ///<summary>
    ///Places the parameter item in the location parameter
    ///<summary>
    public void AddItemToEquipmentLocation(EquipmentLocation loc, EquipableItem item){
        Debug.Assert(item.GetAllowedEquipLocation() == loc);

        equippedItems[loc] = item;

        if(equipmentUpdated != null){
            Debug.Log("Got here in AddItem in EquipItem!");
            equipmentUpdated();
        }
    }

    ///<summary>
    ///Removes item from the location parameter
    ///<summary>
    public void RemoveItemFromEquipmentLocation(EquipmentLocation loc){
        if(equippedItems.TryGetValue(loc, out EquipableItem removeItem)){
                //if there is something to remove
            if(Inventory.GetPlayerInventory().HasSpaceForItem(removeItem)){
                    //if there is room for item
                Inventory.GetPlayerInventory().FindFirstEmptySlot().AddItemToSlot(removeItem, 1);
            } else {
                    Debug.Log("No room for item");
                    //TODO implement solution for having no inventory space
            }
        }

        equippedItems.Remove(loc);
        if (equipmentUpdated != null){
            Debug.Log("Got here in removeItem!");
                equipmentUpdated();
        }
    }

    public void EquipItem(EquipableItem item, int indexFromInventory){
        equippedItems.TryGetValue(item.GetAllowedEquipLocation(), out EquipableItem equippedItem);
        if(equippedItem){
            equippedItems.Remove(item.GetAllowedEquipLocation());
        }
        AddItemToEquipmentLocation(item.GetAllowedEquipLocation(), item);
        Inventory.GetPlayerInventory().FindSlotInInventory(indexFromInventory).RemoveItemFromSlot(1); //equipable items are not stackable, therefore we can remove just one. If this is not called from inventory, the index does not matter
        if(equippedItem){
            Inventory.GetPlayerInventory().FindFirstEmptySlot().AddItemToSlot(equippedItem, 1);
        }

        if (equipmentUpdated != null){
            Debug.Log("Got here in EquipItem!");
            equipmentUpdated();
        }
    }



}
