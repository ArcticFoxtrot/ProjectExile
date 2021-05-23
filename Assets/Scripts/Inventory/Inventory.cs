using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    [SerializeField] int inventorySize;
    [SerializeField] GameObject inventoryParent;
    [SerializeField] GameObject inventorySlotPrefab;

    //list of inventory slots
    public List<Slot> slots = new List<Slot>();

    public event Action inventoryUpdated;

    public static Inventory GetPlayerInventory(){
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void ResizeInventory(int change){
        Debug.Log("Resizing inventory");
        inventorySize -= change;
        slots.Capacity = inventorySize;
    }

    public Slot FindEmptySlot(){
        //if we have not reached maximum inventory, enable new slot
        if(slots.Count < inventorySize){
            ResizeInventory(1);
            return slots[slots.Count];
        } else {
            return null;
        }
    }

    public int GetSize()
    {
        return slots.Count;
    }

    public int GetNumberInSlot(int slotIndex)
    {
        return slots[slotIndex].number;
    }

    public InventoryItem GetItemInSlot(int slotIndex)
    {
        return slots[slotIndex].item;
    }

    public Slot FindFirstEmptySlot(){
        Slot returnSlot = null;
        for (int i = 0; i < slots.Count - 1; i++)
        {
            if(slots[i].GetIsEmptySlot()){
                returnSlot = slots[i];
                break;
            }
        }
        if(returnSlot == null){
            returnSlot = FindEmptySlot();
        }
        return returnSlot;
    }

    public bool HasSpaceForItem(EquipableItem removeItem)
    {
        if(FindFirstEmptySlot() == null){
            return false;
        } else {
            return true;
        }
    }

    private void PopulateInventory(){
        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slotObject = Instantiate(inventorySlotPrefab, inventoryParent.transform);
            slots.Add(slotObject.GetComponent<Slot>());
            slotObject.name = "Slot " + i.ToString();
            slotObject.GetComponent<Slot>().SetInventoryIndex(i); //set the index for the slot
        }
    }

    private void Awake() {
        PopulateInventory();
    }

    public void InventoryUpdated(){
        if(inventoryUpdated != null){
            inventoryUpdated();
        }
    }
    
    public Slot FindSlotInInventory(int index){
        return slots[index];
    }

}
