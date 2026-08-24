using Inventory.Container;
using UnityEngine;
using UnityEngine.EventSystems;


public abstract class InventoryView : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject slotPrefab;
    protected InventorySlot[] slots;
    
    public abstract void CreateSlots();

    private void Start()
    {
        CreateSlots();
    }
    
    public void OnEnter(InventorySlot slot)
    {
        Debug.Log("OnEnter");
    }

    public void OnExit(InventorySlot slot)
    {

    }

    public void OnDragStart(InventorySlot slot)
    {
        
    }

    public void OnDragEnd(InventorySlot slot)
    {
        
    }

    public void OnDrag(InventorySlot slot, PointerEventData eventData)
    {
        
    }
}
