using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIMoveDirection {Up, Down, Left, Right}

public class InventoryUICursor : MonoBehaviour
{

/// <summary>
    /// Controls which inventory slot under the parent object to which this script is attached is pointing at according to user input
    /// </summary>

    Inventory playerInventory;
    [SerializeField] InventoryUI inventoryUI;


    public List<InventorySlotUI> slots = new List<InventorySlotUI>();
    public InventorySlotUI[] slotsArray;
    public int currentSlotIndex = 0;

    public bool isSetup = false;
    public bool isRedrawComplete = false;

    private void Awake() {
        playerInventory = Inventory.GetPlayerInventory();
        Setup();
        playerInventory.inventoryUpdated += OnInventoryUpdated;
        inventoryUI.inventoryRedrawn += OnRedrawCompleted;
    }



    private void OnDestroy() {
       // playerInventory.inventoryUpdated -= OnInventoryUpdated;
        inventoryUI.inventoryRedrawn -= OnRedrawCompleted;
    }

    private void OnInventoryUpdated(){
        isSetup = false;
    }
        
    private void OnRedrawCompleted()
    {
        isRedrawComplete = true;
    }


    private void Update() {
        //get input
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            GetNextSlot(UIMoveDirection.Up);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            GetNextSlot(UIMoveDirection.Down);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            GetNextSlot(UIMoveDirection.Left);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            GetNextSlot(UIMoveDirection.Right);
        }
    }



    private void LateUpdate() {
        if(isSetup == false && isRedrawComplete == true){
            Setup();
        }
    }

    public void Setup() {
        ResetSlots();
        slotsArray = GetComponentsInChildren<InventorySlotUI>();
        if(slotsArray.Length > 0){
            for (int i = 0; i < slotsArray.Length; i++)
        {
            if(slotsArray[i].isBeingDestroyed == false){
                slots.Add(slotsArray[i]);
                slotsArray[i].SetCursorActive(false);
            }
        }
        slots[currentSlotIndex].SetCursorActive(true);
        isSetup = true;
        isRedrawComplete = false;
        }

    }

    public void ResetSlots(){
        if(slots.Count > 0){
            slots.Clear();
        }
        if(slotsArray.Length > 0){
            slotsArray = slotsArray.ClearArray<InventorySlotUI>();
        }        
    }

    private void GetNextSlot(UIMoveDirection direction)
    {
        InventorySlotUI selectableNeighbour = slots[currentSlotIndex].FindNeighbours(direction);
        if(selectableNeighbour != null){
            int previousSlotIndex = currentSlotIndex;
            for (int i = slots.Count - 1; i >= 0; i--)
            {
                if(slots[i] == selectableNeighbour){
                    currentSlotIndex = i;
                } 

            } 
            MoveCursor(previousSlotIndex, currentSlotIndex);
        } else {
                Debug.Log("Neighbour not found!");
        }

    }

    private void MoveCursor(int previousSlotIndex, int currentSlotIndex){
        slots[previousSlotIndex].SetCursorActive(false);
        slots[currentSlotIndex].SetCursorActive(true);
    }

    public int GetCurrentSlotIndex()
    {
        return currentSlotIndex;
    }

    public InventorySlotUI GetCurrentSlot(){
        return slots[currentSlotIndex];
    }

}
