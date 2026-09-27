using Inventory.Container;
using Inventory.Database;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private Image amountBackground;
        [SerializeField] private TextMeshProUGUI amountText;

        private InventorySlot _slot;
        private ItemDatabaseObject _database;

        public void Bind(InventorySlot slot, ItemDatabaseObject database)
        {
            if (_slot != null)
                _slot.OnChanged -= Refresh;
            
            _slot = slot;
            _database = database;

            _slot.OnChanged += Refresh;
            Refresh();
        }

        private void Refresh()
        {
            if (_slot.ID >= 0)
            {
                itemIcon.sprite = _database.GetItem[_slot.ID].uiDisplay;
                itemIcon.color = Color.white;

                if (_slot.amount > 1)
                {
                    amountBackground.color = Color.white;
                    amountText.text = _slot.amount.ToString();
                }
                else
                {
                    amountBackground.color = Color.clear;
                    amountText.text = string.Empty;
                }
                
            }
            else
            {
                itemIcon.sprite = null;
                itemIcon.color = Color.clear;
                amountBackground.color = Color.clear;
                amountText.text = string.Empty;
            }
        }

        public void Unbind()
        {
            if(_slot != null)
                _slot.OnChanged -= Refresh;
            
            _slot = null;
            _database = null;
        }

    }
    
}