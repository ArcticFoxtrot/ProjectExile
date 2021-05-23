using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public InventoryItem item;
    public int number = 1;
    public bool isDestroyed = false;

    Inventory inventory;



    private void Awake() {
        inventory = Inventory.GetPlayerInventory();
    }

    public void Setup(InventoryItem item, int number, PhysChemMaterialType type){
        this.item = item;
        GetComponent<PhysChemMaterial>().Setup(type);
        if(!item.IsStackable()){
            number = 1;
        } else {
            this.number = number;
        }
    }

    public int GetNumber(){
        return number;
    }

    public InventoryItem GetItem(){
        return item;
    }

    public string GetDisplayName(){
        return item.GetDisplayName();
    }

    public void PickupItem(){
        Slot slot = inventory.FindFirstEmptySlot();
        if(slot != null){
            //destroy the pick up object
            slot.AddItemToSlot(item, number);
            Debug.Log("Destroying gameobject " + gameObject.name);
            PickupUI ui = GetComponent<PickupUI>();
            ui.pickupUIIsDestroyed = true;
            ui.DestroyUIElement();
            
            Destroy(gameObject);

        }
    }

}
