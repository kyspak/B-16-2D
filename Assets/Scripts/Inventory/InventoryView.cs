using System;
using Inventory.Container;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class InventoryView : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject slotPrefab;

    [SerializeField] private float _widthDragVisual;
    [SerializeField] private float _heightDragVisual;
    
    protected InventorySlot[] slots;

    private static readonly MouseItem mouseItem = new MouseItem();
    private static GameObject _dragVisual;
    private RectTransform _dragVisualRect;
    private Canvas _dragVisualCanvas;
    private Image _dragVisualImage;

    public virtual void OnLeftClick(InventorySlot slot){}
    public virtual void OnRightClick(InventorySlot slot){}
    
    public abstract void CreateSlots();

    private void Start()
    {
        Debug.Log($"Starting {GetType().Name}");
        CreateSlots();
        _dragVisual = GetDragVisual();
    }

    public void OnEnterInterface(GameObject obj)
    {
        mouseItem.ui = this;
    }

    public void OnExitInterface(GameObject obj)
    {
        mouseItem.ui = null;
    }
    
    public void OnEnter(InventorySlot slot)
    {
        mouseItem.toSlot = slot;
    }

    public void OnExit(InventorySlot slot)
    {
        if (mouseItem != null && mouseItem.toSlot != null)
            mouseItem.toSlot = null;
        

    }

    public void OnDragStart(InventorySlot slot)
    {
        if (slot.ID >= 0)
        {
            _dragVisualImage.sprite = inventory.database.GetItem[mouseItem.toSlot.ID].uiDisplay;
            _dragVisual.SetActive(true);
        }
        mouseItem.obj = _dragVisual;
        mouseItem.toSlot = slot;
        _dragVisualCanvas = mouseItem.obj.GetComponentInParent<Canvas>();
    }

    public void OnDragEnd(InventorySlot fromSlot)
    {
        InventorySlot toSlot = mouseItem.toSlot ?? fromSlot;
        
        inventory.MoveItem(toSlot, fromSlot);
        
        _dragVisual.SetActive(false);
        mouseItem.toSlot = null;
    }

    public void OnDrag(InventorySlot slot, PointerEventData eventData)
    {
        if (!mouseItem.obj)
            return;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _dragVisualRect.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 worldPoint
        );
        
        _dragVisualRect.pivot = worldPoint;
    }

    private GameObject GetDragVisual()
    {
        if (!_dragVisual)
        {
            _dragVisual = new  GameObject("dragVisual");
            var rt = _dragVisual.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(_widthDragVisual, _heightDragVisual);
            _dragVisualImage = _dragVisual.AddComponent<Image>();
            _dragVisualImage.raycastTarget = false;
        }
        
        Canvas rootCanvas = GetComponentInParent<Canvas>();
        
        _dragVisual.transform.SetParent(rootCanvas.transform);
        _dragVisual.transform.SetAsFirstSibling();
        _dragVisual.SetActive(false);
        
        return _dragVisual;
    }
}

public class MouseItem
{
    public InventoryView ui;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot toSlot;
}
