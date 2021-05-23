using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IItemHolder
{

    [SerializeField] InventoryItemIconHandler icon = null;
    [SerializeField] EquipmentLocation equipmentLocation = EquipmentLocation.Head;

    Equipment playerEquipment;


    private void Awake() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerEquipment = player.GetComponent<Equipment>();
        playerEquipment.equipmentUpdated += RefreshEquipmentUI;
    }

    private void Start() {
        RefreshEquipmentUI();
    }

    public int MaxAcceptable(InventoryItem item){
        EquipableItem equipableItem = item as EquipableItem;
        if(equipableItem == null){
            return 0;
        }
        if(equipableItem.GetAllowedEquipLocation() != equipmentLocation) {
            return 0;
        }
        if(GetItem() != null){
            return 0;
        }

        return 1;
    }

    public void AddItems(InventoryItem item, int number){
        playerEquipment.AddItemToEquipmentLocation(equipmentLocation, (EquipableItem) item);
    }

    public InventoryItem GetItem()
    {
        return playerEquipment.GetItemInSlot(equipmentLocation);
    }

    public int GetNumber(){
        if (GetItem() != null){
                return 1;
        } else {
                return 0;
        }
    }

    public void RemoveItems(int number){
        playerEquipment.RemoveItemFromEquipmentLocation(equipmentLocation);
    }

    void RefreshEquipmentUI()
    {
        icon.SetItem(playerEquipment.GetItemInSlot(equipmentLocation), 0);
    }
}
