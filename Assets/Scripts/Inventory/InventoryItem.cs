using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : ScriptableObject
{
    [SerializeField] string itemID = null;
    [SerializeField] string displayName = null;
    [SerializeField] string description = null;
    [SerializeField] Sprite icon;
    [SerializeField] Pickup pickup;
    [SerializeField] bool isStackable;
    [SerializeField] PhysChemMaterialType physChemMaterialType;

    public bool IsStackable()
    {
        return isStackable;
    }

    public Pickup SpawnPickup(Vector3 position, int number)
    {
        Pickup pickup = Instantiate(this.pickup);
        pickup.transform.position = position;
        pickup.Setup(this, number, physChemMaterialType);
        return pickup;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetDisplayName(){
        return displayName;
    }
}
