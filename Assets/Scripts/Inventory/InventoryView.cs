<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
using Inventory.Container;
using UI.SeparatePanel;
using UnityEngine;
using UnityEngine.EventSystems;
<<<<<<< Updated upstream
=======
using UnityEngine.InputSystem;
using UnityEngine.UI;
>>>>>>> Stashed changes

namespace Inventory
{
    public abstract class InventoryView : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject slotPrefab;
<<<<<<< Updated upstream
    protected InventorySlot[] slots;
=======

    
        
    [SerializeField] private Vector2 _spriteSizeVisual = new Vector2(100, 100);
    
    protected InventorySlot[] slots;

    private static readonly MouseItem mouseItem = new MouseItem();
    private static GameObject _dragVisual;
    private RectTransform _dragVisualRect;
    private Canvas _dragVisualCanvas;
    private static Image _dragVisualImage;
    private bool _isManualDragActive;

    public virtual void OnLeftClick(InventorySlot slot){}
    public virtual void OnRightClick(InventorySlot slot){}
>>>>>>> Stashed changes
    
    public abstract void CreateSlots();
    

    private void Start()
    {
        CreateSlots();
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes
    }
    
    public void OnEnter(InventorySlot slot)
    {
        Debug.Log("OnEnter");
    }

    public void OnExit(InventorySlot slot)
    {
<<<<<<< Updated upstream

=======
        if (mouseItem != null && mouseItem.toSlot != null)
            mouseItem.toSlot = null;
>>>>>>> Stashed changes
    }

    public void OnDragStart(InventorySlot slot)
    {
<<<<<<< Updated upstream
        
=======
        if (slot.ID >= 0)
        {
            _dragVisualImage.sprite = inventory.database.GetItem[slot.ID].uiDisplay;
            _dragVisual.SetActive(true);
        }
        mouseItem.obj = _dragVisual;
        mouseItem.toSlot = slot;
        _dragVisualCanvas = mouseItem.obj.GetComponentInParent<Canvas>();
        _dragVisualRect = mouseItem.obj.GetComponent<RectTransform>();
>>>>>>> Stashed changes
    }

    public void OnDragEnd(InventorySlot slot)
    {
        
    }

    public void OnDrag(InventorySlot slot, PointerEventData eventData)
    {
        
<<<<<<< Updated upstream
=======
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
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======

public class MouseItem
{
    public InventoryView ui;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot toSlot;
}
}

>>>>>>> Stashed changes
