using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slot : MonoBehaviour
{

    public bool isEmptySlot = true;
    public InventoryItem item;
    public int number;
    public int inventoryIndex;

    private Inventory playerInventory;

    //event for changes in inventory
    public event Action inventoryUpdated;

    private void Awake() {
        playerInventory = Inventory.GetPlayerInventory();
    }

    public virtual void AddItemToSlot(InventoryItem item, int number){
        this.item = item;
        this.number = number;
        isEmptySlot = false;
        //tell playerInventory that something has been added
        playerInventory.InventoryUpdated();
    }

    public void RemoveItemFromSlot(int number){
        this.item = null;
        this.number -= number;
        if(this.number <= 0){
            this.number = 0;
            this.item = null;
            isEmptySlot = true;
        }
        //tell playerInventory that something has been removed
        playerInventory.InventoryUpdated();
    }

    public bool GetIsEmptySlot(){
        return isEmptySlot;
    }

    public void SetInventoryIndex(int index){
        inventoryIndex = index;
    }

    public int GetInventoryIndex(){
        return inventoryIndex;
    }

}
