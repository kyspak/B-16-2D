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

        public static void BindInventoryUIEvent(GameObject obj, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnEnterInterface(obj); });
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { ui.OnExitInterface(obj); });
        }

        public static void BindClickEvent(GameObject obj, InventorySlot slot, InventoryView ui)
        {
            AddEvent(obj, EventTriggerType.PointerClick, (eventData) =>
            {
                //var pointer = eventData as PointerEventData;
                
                PointerEventData pointerData = (PointerEventData)eventData;

                switch (pointerData.button)
                {
                    case PointerEventData.InputButton.Left:
                        ui.OnLeftClick(slot);
                        break;
                    case PointerEventData.InputButton.Right:
                        ui.OnRightClick(slot);
                        break;
                }
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