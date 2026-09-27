using UnityEngine;
using Inventory.Container;
using Inventory.Database;

namespace Inventory
{
    public static class InventorySlotBinder
    {
        public static void Bind(GameObject obj,
            InventorySlotView view,
            InventorySlot slot,
            InventoryView ui,
            ItemDatabaseObject database)
        {
            SlotEventBinder.BindSlotEvent(obj, slot, ui);
            SlotEventBinder.BindClickEvent(obj, slot, ui);
            view.Bind(slot, database);
            slot.Bind(database);
        }
    }
}