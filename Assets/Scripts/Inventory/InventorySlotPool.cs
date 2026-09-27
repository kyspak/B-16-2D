
using System.Collections.Generic;
using UnityEngine;



namespace Inventory
{
    public class PooledSlot
    {
        public GameObject GameObject { get; private set; }
        public InventorySlotView View { get; private set; }

        public PooledSlot(GameObject gameObject)
        {
            GameObject = gameObject;
            View = gameObject.GetComponent<InventorySlotView>();
        }
        
    }

    public class InventorySlotPool
    {
        private readonly GameObject _slotPrefab;
        private readonly Transform _poolRoot;
        private readonly Queue<PooledSlot> _pool = new();

        public InventorySlotPool(GameObject slotPrefab, Transform poolRoot, int capacity)
        {
            _slotPrefab = slotPrefab;
            _poolRoot = poolRoot;

            for (int i = 0; i < capacity; i++)
            {
                var obj = Object.Instantiate(slotPrefab, poolRoot);
                obj.SetActive(false);
                _pool.Enqueue(new PooledSlot(obj));
            }
        }

        public PooledSlot Rent(Transform parent)
        {
            PooledSlot pooled = _pool.Count > 0 
                ? _pool.Dequeue() 
                : new PooledSlot(Object.Instantiate(_slotPrefab));
            
            pooled.GameObject.transform.SetParent(parent, false);
            pooled.GameObject.SetActive(true);
            return pooled;
        }

        public void Return(PooledSlot pooled)
        {
            SlotEventBinder.Unbind(pooled.GameObject);
            pooled.View.Unbind();
            pooled.GameObject.SetActive(false);
            pooled.GameObject.transform.SetParent(_poolRoot, false);
            _pool.Enqueue(pooled);
            
        }
    }
}