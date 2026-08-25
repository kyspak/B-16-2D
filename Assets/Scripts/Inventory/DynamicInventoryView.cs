using UnityEngine;

namespace Inventory
{
    public class DynamicInventoryView : InventoryView
    {
        public override void CreateSlots()
        {
            slots = inventory.container.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                var obj = Instantiate(slotPrefab, transform);
                SlotEventBinder.BindSlotEvent(obj, slots[i], this);
                var view = obj.GetComponent<InventorySlotView>();
                view.Bind(slots[i], inventory.database);
            }
        }
    }
}