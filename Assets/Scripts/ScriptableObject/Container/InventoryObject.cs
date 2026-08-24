
using System;
using Inventory.Database;
using Inventory.Items;
using UnityEngine;

namespace Inventory.Container
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {

        public ItemDatabaseObject database;
        public Inventory container;
        
        public void AddItem(Items.Item item, int amount)
        {
            int remaining = amount;

            // int[] emptySlotID = new int[container.Items.Length];
            
            for (int i = 0; i < container.Items.Length; i++)
            {
                
                if (amount <= 0)
                    return;
                //
                // if (container.Items[i].ID == -1)
                //       emptySlotID[i] = i;
                
                if (container.Items[i].ID != item.Id)
                    continue;
                
                
                
                int space = container.Items[i].item.MaxStack - container.Items[i].amount;
                if (space <= 0)
                    continue;
                
                int toAdd = Math.Min(space, remaining);
                container.Items[i].AddAmount(toAdd);
                remaining -= toAdd;
            }

            // if (remaining > 0 && emptySlotID[0] != 0)
            // {
            //     for (int i = 0; i < emptySlotID.Length; i++)
            //     {
            //         if (remaining == 0)
            //             return;
            //         
            //         int toAdd = Math.Min(item.MaxStack, remaining);
            //         container.Items[i].UpdateSlot(item.Id, toAdd, database);
            //         remaining -= toAdd;
            //         int lastEmptySlot = emptySlotID[i];
            //     }
            //     
            // }
            
            while (remaining > 0)
            {
                int toAdd = Math.Min(item.MaxStack, remaining);
                bool placed = SetEmptySlot(item, toAdd);

                if (!placed)
                    return;
                remaining -= toAdd;
            }
            
            
        }

        public void RemoveItem(InventorySlot slot)
        {
            slot.UpdateSlot(-1, 0, database);
        }

        public void MoveItem(InventorySlot toSlot, InventorySlot fromSlot)
        {
            if (ReferenceEquals(toSlot, fromSlot))
                return;

            if (toSlot.ID == fromSlot.ID)
            {
                int amount = toSlot.amount + fromSlot.amount;
                if (toSlot.item.MaxStack > amount)
                {
                    toSlot.AddAmount(fromSlot.amount);
                    RemoveItem(fromSlot);
                    return;
                }
                
                int space = toSlot.item.MaxStack - toSlot.amount;
                toSlot.AddAmount(space);
                fromSlot.AddAmount(-space);
                return;
            }
            
            InventorySlot temp = new InventorySlot(fromSlot.ID, fromSlot.amount);
            
            fromSlot.UpdateSlot(toSlot.ID, toSlot.amount, database);
            toSlot.UpdateSlot(temp.ID, temp.amount, database);
        }

        [ContextMenu("Clear Slots")]
        public void ClearSlots()
        {
            foreach (var slot in container.Items)
            {
                slot.UpdateSlot(-1, 0, database);
            }
        }
        
        private bool SetEmptySlot(Items.Item item, int value)
        {
            foreach (var slot in container.Items)
            {
                if (slot.ID <= -1)
                {
                    slot.UpdateSlot(item.Id, value, database);
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class Inventory
    {
        public InventorySlot[] Items;
    }

    [Serializable]
    public class InventorySlot
    {
        [field: NonSerialized] public event Action OnChanged;
        
        [NonSerialized]private Items.Item _cachedItem;
        public Items.Item item => _cachedItem;
        
        public int ID = -1;
        public int amount;

        public InventorySlot()
        {
            ID = -1;
            amount = 0;
        }

        public InventorySlot(int id, int amount)
        {
            ID = id;
            this.amount = amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }

        

        public void UpdateSlot(int id, int value, ItemDatabaseObject database)
        {
            ID = id;
            amount = value;
            RefreshCache(database);
            OnChanged?.Invoke();
        }

        public bool CanPlaceInSlot(ItemsObject _item)
        {
            if(ID <= -1 && amount <= 0) 
                return true;
            return false;
        }

        private void RefreshCache(ItemDatabaseObject database)
        {
            _cachedItem = ID >= 0 ? database.GetItem[ID].CreateItem() : null;
        }
    }
}