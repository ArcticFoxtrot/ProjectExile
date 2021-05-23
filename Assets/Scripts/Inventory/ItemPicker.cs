using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{

    [SerializeField] float rangeRefreshInSeconds;
    [SerializeField] float reachableDistanceSqr;
    [SerializeField] float holdToEquipInputTime = 1f;
    //[SerializeField] InteractionUISpawner interactionUISpawner;
    //[SerializeField] InteractionUIHandler interactionUIHandler;

    private bool pickInput = false;
    private bool holdToEquipInputStart = false;
    private bool holdToEquipInputEnd = false;
    private float holdStartTime;
    private float holdEndTime;

    public List<Pickup> reachablePickups = new List<Pickup>();

    private Equipment playerEquipment;
    private Inventory playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GetComponent<Inventory>();
        if(!playerInventory || playerInventory == null){
            Debug.LogWarning("Item Picker did not find an Inventory component on Player");
        }
        playerEquipment = GetComponent<Equipment>();
        if(!playerEquipment || playerEquipment == null){
            Debug.LogWarning("Item Picker did not find an Equipment component on Player");
        }
        StartCoroutine(CheckForPickups());
    }

    // Update is called once per frame
    void Update()
    {
        pickInput = Input.GetKeyDown(KeyCode.Q);
        if(pickInput){
            PickClosestItem();
        }

        holdToEquipInputStart = Input.GetKeyDown(KeyCode.R);
        if(holdToEquipInputStart){
            holdStartTime = Time.time;
        }

        holdToEquipInputEnd = Input.GetKeyUp(KeyCode.R);
        if(holdToEquipInputEnd){
            holdEndTime = Time.time;
            float timeDiff = holdEndTime - holdStartTime;
            if(timeDiff > holdToEquipInputTime){
                //equip in appropriate slot
                EquipClosestItem();
            }

        }
    }


    IEnumerator CheckForPickups(){
        reachablePickups.Clear();
        
        Pickup[] pickupsArray = FindObjectsOfType<Pickup>();
        List<Pickup> allPickups = new List<Pickup>(pickupsArray);
        
        if(pickupsArray.Length > 0){
            reachablePickups = GetPickupsInRadius(pickupsArray);
            //interactionUIHandler.HandlePickUpsInRange(reachablePickups);
            if(reachablePickups.Count > 0){
                HandlePickupsInRange(reachablePickups);
            }
            
            HandlePickupsOutOfRange(reachablePickups, allPickups);
        }
        
        yield return new WaitForSeconds(rangeRefreshInSeconds);
        StartCoroutine(CheckForPickups());
    }

    private List<Pickup> GetPickupsInRadius(Pickup[] pickupArray){
        List<Pickup> returnList = new List<Pickup>();
        for (int i = 0; i < pickupArray.Length; i++)
        {
            float distanceSqr = (transform.position - pickupArray[i].transform.position).sqrMagnitude;
            if(distanceSqr < reachableDistanceSqr){
                returnList.Add(pickupArray[i]);
            }
        }

        return returnList;
    }

    public void PickClosestItem(){
        //sort reachable pickups by distance from player
        reachablePickups.Sort(
            delegate(Pickup a, Pickup b){
                float aDistSqr = (this.transform.position - a.transform.position).sqrMagnitude;
                float bDistSqr = (this.transform.position - b.transform.position).sqrMagnitude;
                return aDistSqr.CompareTo(bDistSqr);
            }
            );

        reachablePickups[0].PickupItem();
        RemoveClosestItemFromReachable();
 
    }

    public void EquipClosestItem(){
        //sort reachable pickups by distance from player
        //TODO refactor this to not have same effect as picking up?
        reachablePickups.Sort(
            delegate(Pickup a, Pickup b){
                float aDistSqr = (this.transform.position - a.transform.position).sqrMagnitude;
                float bDistSqr = (this.transform.position - b.transform.position).sqrMagnitude;
                return aDistSqr.CompareTo(bDistSqr);
            }
            );

        reachablePickups[0].PickupItem();
        EquipableItem equipableItem = reachablePickups[0].GetItem() as EquipableItem;
        //playerEquipment.EquipItem(equipableItem, playerInventory.FindSlot(reachablePickups[0].GetItem()) - 1);
        playerEquipment.EquipItem(equipableItem, playerInventory.FindFirstEmptySlot().GetInventoryIndex() - 1);
        RemoveClosestItemFromReachable();
    }

    private void RemoveClosestItemFromReachable(){
        reachablePickups[0].GetComponent<PickupUI>().SetUIElementActive(false);
        reachablePickups.Remove(reachablePickups[0]);
    }


    private void HandlePickupsInRange(List<Pickup> reachablePickups)
    {
        for (int i = 0; i < reachablePickups.Count; i++)
        {
            
            if(reachablePickups[i].isDestroyed == false){
                if(reachablePickups[i].TryGetComponent<PickupUI>(out PickupUI pui)){
                    pui.SetUIElementActive(true);
                }
                //reachablePickups[i].GetComponent<PickupUI>().SetUIElementActive(true);
            }
        }
    }

    private void HandlePickupsOutOfRange(List<Pickup> reachablePickups, List<Pickup> allPickups)
    {

        if(reachablePickups.Count < 1){
            foreach(Pickup p in allPickups){
               p.GetComponent<PickupUI>().SetUIElementActive(false);
           } 
           return;
        }

        foreach(Pickup p in allPickups){
            if(reachablePickups.Contains(p) == false){
                p.GetComponent<PickupUI>().SetUIElementActive(false);
            }
            
        }
    }
}
