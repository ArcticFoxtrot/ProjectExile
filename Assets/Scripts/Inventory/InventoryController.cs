using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] InventoryUICursor inventoryUICursor;

    public InventorySlotUI startMoveSlot;
    public InventorySlotUI endMoveSlot;

    private bool moveInput = false;
    private bool moveStarted = false;
    private bool dropInput = false;
    private bool equipInput = false;

    private Inventory playerInventory;
    private ItemDropper playerItemDropper;
    private Equipment playerEquipment;
    

    private void Start() {
        playerInventory = Inventory.GetPlayerInventory();
        playerItemDropper = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemDropper>();
        playerEquipment = GameObject.FindGameObjectWithTag("Player").GetComponent<Equipment>();
    }

    private void Update() {
        //get selection input
        moveInput = Input.GetKeyDown(KeyCode.E);
        dropInput = Input.GetKeyDown(KeyCode.G);
        equipInput = Input.GetKeyDown(KeyCode.R);
        if(moveInput && moveStarted == false){
            StartMove();
        } else if(moveInput && moveStarted == true){
            EndMove();
        }

        if(dropInput){
            //DropItemFromInventory();
            int index = inventoryUICursor.GetCurrentSlotIndex();
            InventoryItem dropItem = playerInventory.GetItemInSlot(index);
            int dropNumber = playerInventory.GetNumberInSlot(index);
            if(dropItem != null){
                playerItemDropper.DropItem(dropItem, dropNumber);
                playerInventory.FindSlotInInventory(index).RemoveItemFromSlot(dropNumber);
            }
           //Debug.Log("Item in the current slot is " + playerInventory.GetItemInSlot(inventoryUICursor.GetCurrentSlotIndex()));
        }

        if(equipInput){
            int index = inventoryUICursor.GetCurrentSlotIndex();
            EquipableItem selectedItem = playerInventory.GetItemInSlot(index) as EquipableItem;
            if(selectedItem != null){
                playerEquipment.EquipItem(selectedItem, index);
            }

        }

    }

    private void StartMove(){
        startMoveSlot = inventoryUICursor.GetCurrentSlot();
        if(startMoveSlot.GetItem() != null){
            moveStarted = true;
        }
    }

        private void EndMove()
    {
        endMoveSlot = inventoryUICursor.GetCurrentSlot();

        DropItemIntoContainer(endMoveSlot);
        moveStarted = false;
        startMoveSlot = null;
        endMoveSlot = null;
    }

    private void DropItemFromInventory(){
        DropItemIntoContainer(null);
        
    }


    private void DropItemIntoContainer(InventorySlotUI destination)
        {
            if (destination != null && object.ReferenceEquals(destination, startMoveSlot)) return;

            var destinationContainer = destination;
            var sourceContainer = startMoveSlot;

            if(sourceContainer.GetItem() == null) return;

            
            // Swap won't be possible
            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                return;
            }

            AttemptSwap(destinationContainer, sourceContainer);
        }

    private void AttemptSwap(InventorySlotUI destination, InventorySlotUI source)
        {
            
            // Provisionally remove item from both sides. 
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            var removedDestinationNumber = destination.GetNumber();
            var removedDestinationItem = destination.GetItem();

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            // Do take backs (if needed)
            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            // Abort if we can't do a successful swap
            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber ||
                removedSourceNumber == 0)
            {
                if (removedDestinationNumber > 0)
                {
                    destination.AddItems(removedDestinationItem, removedDestinationNumber);
                }
                if (removedSourceNumber > 0)
                {
                    source.AddItems(removedSourceItem, removedSourceNumber);
                }
                return;
            }

            // Do swaps
            if (removedDestinationNumber > 0)
            {
                source.AddItems(removedDestinationItem, removedDestinationNumber);
            }
            if (removedSourceNumber > 0)
            {
                destination.AddItems(removedSourceItem, removedSourceNumber);
            }
        }

    private bool AttemptSimpleTransfer(InventorySlotUI destination)
        {
            var draggingItem = startMoveSlot.GetItem();
            var draggingNumber = startMoveSlot.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                startMoveSlot.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }

            return true;
        }

    private int CalculateTakeBack(InventoryItem removedItem, int removedNumber, InventorySlotUI removeSource, InventorySlotUI destination)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = removeSource.MaxAcceptable(removedItem);

                // Abort and reset
                if (sourceTakeBackAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }
            return takeBackNumber;
        }

}
