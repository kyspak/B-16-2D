using System;
using Inventory.Container;
using UnityEngine;

namespace UI.SeparatePanel
{
    public class SplitWindowController : MonoBehaviour
    {
        [SerializeField] private ItemSplitWindow _splitWindow;
        
        public void RequestSplit(InventorySlot source, int maxAmount, Action<int> onAmountChosen)
        {
            _splitWindow.ShowWindow(maxAmount, onAmountChosen);
        }
    }
}