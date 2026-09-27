
using Inventory.Container;
using UI.SeparatePanel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory
{
    public abstract class InventoryView : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject slotPrefab;
    protected InventorySlot[] slots;

    
        
    [SerializeField] private Vector2 _spriteSizeVisual = new Vector2(100, 100);
    

    private static readonly MouseItem mouseItem = new MouseItem();
    private static GameObject _dragVisual;
    private RectTransform _dragVisualRect;
    private Canvas _dragVisualCanvas;
    private static Image _dragVisualImage;
    private bool _isManualDragActive;

    public virtual void OnLeftClick(InventorySlot slot){}
    public virtual void OnRightClick(InventorySlot slot){}
    
    public abstract void CreateSlots();
    

    private void Start()
    {
        CreateSlots();
        SlotEventBinder.BindInventoryUIEvent(gameObject, this);
        _dragVisual = GetDragVisual();
    }

    public virtual void ClearSlots()
    {
        
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
        Debug.Log("OnEnter");
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
            _dragVisualImage.sprite = inventory.database.GetItem[slot.ID].uiDisplay;
            _dragVisual.SetActive(true);
        }
        mouseItem.obj = _dragVisual;
        mouseItem.toSlot = slot;
        _dragVisualCanvas = mouseItem.obj.GetComponentInParent<Canvas>();
        _dragVisualRect = mouseItem.obj.GetComponent<RectTransform>();
    }

    public void OnDragEnd(InventorySlot slot)
    {
        
    }

    public void OnDrag(InventorySlot slot, PointerEventData eventData)
    {
        
        _dragVisualRect.position = worldPoint;
    }

    private GameObject GetDragVisual()
    {
        if (!_dragVisual)
        {
            _dragVisual = new  GameObject("dragVisual");
            var rt = _dragVisual.AddComponent<RectTransform>();
            rt.sizeDelta = _spriteSizeVisual;
            _dragVisualImage = _dragVisual.AddComponent<Image>();
            _dragVisualImage.raycastTarget = false;
        }
        
        Canvas rootCanvas = GetComponentInParent<Canvas>();
        
        _dragVisual.transform.SetParent(rootCanvas.transform);
        _dragVisual.transform.SetAsLastSibling();
        _dragVisual.SetActive(false);
        
        return _dragVisual;
    }
    
    protected void StartManualDrag(int itemId)
    {
        var img = _dragVisual.GetComponent<Image>();
        img.sprite = inventory.database.GetItem[itemId].uiDisplay;
        img.raycastTarget = false;
        _dragVisual.SetActive(true);
        mouseItem.ui = this;
        _isManualDragActive = true;
    }

    protected void StopManualDrag()
    {
        _dragVisual.SetActive(false);
        _isManualDragActive = false;
        mouseItem.toSlot = null;
    }

    private void Update()
    {
        if (!_isManualDragActive)
            return;
        
        Vector2 pointerPosition = Mouse.current.position.ReadValue();
        var rt = _dragVisual.GetComponent<RectTransform>();
        var canvas = rt.GetComponentInParent<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            rt.position = pointerPosition;
        }
        else
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rt.parent as RectTransform,
                pointerPosition,
                canvas.worldCamera,
                out Vector3 worldPoint);
            
            rt.position = worldPoint;
        }
    }
}

public class MouseItem
{
    public InventoryView ui;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot toSlot;
}
}

