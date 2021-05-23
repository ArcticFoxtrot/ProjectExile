using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// Spawns inventory items when game is first loaded.
    /// </summary>
public class PickupSpawner : MonoBehaviour
{

    [SerializeField] InventoryItem item;
    [SerializeField] int number;


    private void Awake() {
        //spawn pickup in awake
        SpawnPickup();
    }

    private void SpawnPickup(){
        Pickup spawnedPickup = item.SpawnPickup(transform.position, number);
        spawnedPickup.transform.SetParent(transform);
        spawnedPickup.GetComponent<PickupUI>().Setup();
    }

    public Pickup GetPickup(){
        return GetComponentInChildren<Pickup>();
    }

    public bool GetIsCollected(){
        return GetPickup() == null;
    }

    private void DestroyPickup(){
        if(GetPickup()){
            Destroy(GetPickup().gameObject);
        }
    }



}
