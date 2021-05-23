using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IItemHolder
{

    // CONFIG DATA
        [SerializeField] InventoryItemIconHandler iconHandler = null; //use this to set the icon of the image
        [SerializeField] GameObject cursorImageObject;

        // STATE
        int index;
        InventoryItem item;
        Inventory inventory;
        Selectable selectable;

        public bool isBeingDestroyed = false;
       

        // PUBLIC
        private void Awake() {
            selectable = GetComponent<Selectable>();
        }

        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            iconHandler.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (inventory.FindFirstEmptySlot() != null)
            {
                return int.MaxValue;
            }
            return 0;
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.FindSlotInInventory(index).RemoveItemFromSlot(number);
        }

        public void SetCursorActive(bool t){
            cursorImageObject.SetActive(t);
        }

        public void AddItems(InventoryItem item, int number)
        {
            //inventory.AddItemToSlot(index, item, number);
            inventory.FindSlotInInventory(index).AddItemToSlot(item, number);
        }

        public InventorySlotUI FindNeighbours(UIMoveDirection dir){
            InventorySlotUI s = null;
            if(dir == UIMoveDirection.Down){
                if(selectable.FindSelectableOnDown().TryGetComponent<InventorySlotUI>(out InventorySlotUI outSlot)){
                    s = outSlot;
                } 
            } else if (dir == UIMoveDirection.Up){
               
                if(selectable.FindSelectableOnUp().TryGetComponent<InventorySlotUI>(out InventorySlotUI outSlot)){
                    s = outSlot;
                }
            } else if(dir == UIMoveDirection.Right){
                
                if(selectable.FindSelectableOnRight().TryGetComponent<InventorySlotUI>(out InventorySlotUI outSlot)){
                    s = outSlot;
                }
            } else if(dir == UIMoveDirection.Left){
                
                if(selectable.FindSelectableOnLeft().TryGetComponent<InventorySlotUI>(out InventorySlotUI outSlot)){
                    s = outSlot;
                }
            }
            return s;
        }
}
