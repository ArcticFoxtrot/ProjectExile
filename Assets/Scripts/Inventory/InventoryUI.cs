using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryUI : MonoBehaviour
{
    
    [SerializeField] InventorySlotUI inventorySlotUIPrefab;

    Inventory playerInventory;

    public event Action inventoryRedrawn;

    private void Awake() {
        playerInventory = Inventory.GetPlayerInventory();
        playerInventory.inventoryUpdated += RedrawInventory;
    }

    private void RedrawInventory()
    {
        foreach(Transform t in transform){
            //destroy all child objects
            t.gameObject.GetComponent<InventorySlotUI>().isBeingDestroyed = true;
            Destroy(t.gameObject);
        }

        for (int i = 0; i < playerInventory.GetSize(); i++)
        {
            InventorySlotUI slotUI = Instantiate(inventorySlotUIPrefab, transform);
            slotUI.Setup(playerInventory, i);
        }

        if(inventoryRedrawn!= null){
            inventoryRedrawn();
        }
    }
}
