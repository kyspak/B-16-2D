using System;
using Inventory.Container;
using UnityEngine;

namespace Inventory
{
    public class StaticInventory : InventoryView
    {
        private GameObject[] _staticSlot;
        
        public event Action<InventorySlot> OnItemSelected;
        
        
        public override void CreateSlots()
        {
            var items = inventory.container.Items;

            for (int i = 0; i < _staticSlot.Length; i++)
            {
                SlotEventBinder.BindSlotEvent(_staticSlot[i], items[i], this);
                SlotEventBinder.BindClickEvent(_staticSlot[i], items[i], this );
                _staticSlot[i].GetComponent<InventorySlotView>().Bind(items[i], inventory.database);
            }
        }

        public override void OnLeftClick(InventorySlot slot)
        {
            if (slot.item == null)
            {
                return;
            }
            
            OnItemSelected?.Invoke(slot);
        }

        public override void OnRightClick(InventorySlot slot)
        {
            if (slot.item == null)
            {
                return;
            }
            
            OnItemSelected?.Invoke(slot);
        }
    }
}