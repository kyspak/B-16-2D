using Inventory.Container;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public static class SlotEventBinder
    {
        public static void BindSlotEvent(GameObject obj, InventorySlot slot, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnter(slot); });
            AddEvent(obj, EventTriggerType.PointerExit,  delegate { ui.OnExit(slot); });
            AddEvent(obj, EventTriggerType.BeginDrag,    delegate { ui.OnDragStart(slot); });
            AddEvent(obj, EventTriggerType.EndDrag,      delegate { ui.OnDragEnd(slot); });
            AddEvent(obj, EventTriggerType.Drag, (eventData) =>
            {
                ui.OnDrag(slot, (PointerEventData)eventData);
            });

        }

        private static void AddEvent(GameObject obj,
            EventTriggerType type,
            UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            
            if(!trigger)
                trigger = obj.AddComponent<EventTrigger>();

            var entry = new EventTrigger.Entry
            {
                eventID = type
            };
            
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }
    }
}