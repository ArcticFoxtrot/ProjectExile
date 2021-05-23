using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equippable Item", menuName = "Inventory Item/Equipable")]
public class EquipableItem : InventoryItem
{

[SerializeField] EquipmentLocation allowedEquipLocation = EquipmentLocation.MainHand;

    public EquipmentLocation GetAllowedEquipLocation()
    {
        return allowedEquipLocation;
    }
}
