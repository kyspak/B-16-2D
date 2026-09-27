
using System;
using System.Data;
using System.Runtime.CompilerServices;
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
            
            for (int i = 0; i < container.Items.Length; i++)
            {
                
                if (amount <= 0)
                    return;

                if (container.Items[i].ID != item.Id)
                    continue;
                
                
                
                int space = container.Items[i].item.MaxStack - container.Items[i].amount;
                if (space <= 0)
                    continue;
                
                int toAdd = Math.Min(space, remaining);
                container.Items[i].AddAmount(toAdd);
                remaining -= toAdd;
            }
            
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

        public bool SplitItem(InventorySlot fromSlot, InventorySlot toSlot, int requestAmount)
        {
            if (ReferenceEquals(fromSlot, toSlot))
                return false;

            if (fromSlot.item != null && !toSlot.CanPlaceInSlot(fromSlot.item))
            {
                Debug.LogError("Нельзя это положить сюда");
                return false;
            }

            if (requestAmount <= 0 || requestAmount > fromSlot.amount)
                return false;
            
            bool isSameItems = toSlot.ID == fromSlot.ID;
            bool isEmptySlot = toSlot.ID == -1;

            if (!isSameItems && !isEmptySlot)
                return false;

            int amountActual;

            if (isEmptySlot)
            {
                amountActual = requestAmount;
                toSlot.UpdateSlot(fromSlot.ID, amountActual, database);
            }
            else
            {
                int space = fromSlot.item.MaxStack - fromSlot.amount;
                if (space <= 0)
                    return false;
                
                amountActual = Math.Min(space, requestAmount);
                toSlot.AddAmount(amountActual);
            }
            fromSlot.AddAmount(-amountActual);
            
            if(fromSlot.amount <= 0)
                RemoveItem(fromSlot);
            
            return true;
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
            OnChanged?.Invoke();
        }

        

        public void UpdateSlot(int id, int value, ItemDatabaseObject database)
        {
            ID = id;
            amount = value;
            RefreshCache(database);
            OnChanged?.Invoke();
        }

        public bool CanPlaceInSlot(Items.Item _item)
        {
            if(ID <= -1 && amount <= 0) 
                return true;
            return false;
        }

        public void Bind(ItemDatabaseObject database)
        {
            RefreshCache(database);
        }

        private void RefreshCache(ItemDatabaseObject database)
        {
            _cachedItem = ID >= 0 ? database.GetItem[ID].CreateItem() : null;
        }
    }
}