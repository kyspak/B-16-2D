
using Inventory.Container;
using Inventory.Database;
using Inventory.Items;
using NUnit.Framework;
using PlasticGui.WorkspaceWindow.Items;
using UnityEngine;

using InventoryContainerData = Inventory.Container.Inventory;

namespace Inventory.Tests
{
    public class TestItemsObject : ItemsObject
    {
        
    }
    public class InventoryObjectTests
    {
        private const int SlotCount = 4;
        
        private ItemDatabaseObject _database;
        private InventoryObject _inventory;

        [SetUp]
        public void Setup()
        {
            _database = BuildDatabase(
                (id: 0, name: "Wood", maxStack: 10),
                (id: 1, name: "Stone", maxStack: 5)
                );
            _inventory = ScriptableObject.CreateInstance<InventoryObject>();
            _inventory.database = _database;
            _inventory.container = new InventoryContainerData
            {
                Items = BuildEmptySlots(SlotCount)
            };
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_inventory);

            foreach (var item in _database.Items)
            {
                if (item)
                {
                    Object.DestroyImmediate(item);
                }
                
            }
            
            Object.DestroyImmediate(_database);
        }
        // ------helpers-------

        private static ItemDatabaseObject BuildDatabase(params (int id, string name, int maxStack)[] defs)
        {
            var db = ScriptableObject.CreateInstance<ItemDatabaseObject>();
            db.Items = new ItemsObject[defs.Length];

            for (int i = 0; i < defs.Length; i++)
            {
                var item = ScriptableObject.CreateInstance<TestItemsObject>();
                item.Id = defs[i].id;
                item.Name = defs[i].name;
                item.MaxStack = defs[i].maxStack;
                db.Items[i] = item;
            }
            db.OnAfterDeserialize();
            return db;
        }

        private static InventorySlot[] BuildEmptySlots(int count)
        {
            var slots = new InventorySlot[count];
            for (int i = 0; i < count; i++)
                slots[i] = new InventorySlot();
            return slots;
        }

        private Items.Item Wood => _database.GetItem[0].CreateItem();
        private Items.Item Stone => _database.GetItem[1].CreateItem();
        
        //----------------------- AddItem -----------------------

        [Test]
        public void AddItem_ToEmptyInventory_PlacesInFirstEmptySlot()
        {
            _inventory.AddItem(Wood, 3);
            
            Assert.AreEqual(0, _inventory.container.Items[0].ID);
            Assert.AreEqual(3, _inventory.container.Items[0].amount);
        }

        [Test]
        public void AddItem_WithExistingPartialStack_MergesBeforeUsingEmptySlot()
        {
            _inventory.container.Items[0].UpdateSlot(0, 4, _database); // Wood, 4/10
            
            _inventory.AddItem(Wood, 3);
            
            Assert.AreEqual(7, _inventory.container.Items[0].amount);
            Assert.AreEqual(-1, _inventory.container.Items[1].ID); //untouched
        }

        [Test]
        public void AddItem_OverflowingSingleStack_SpillsIntoNewSlot()
        {
            _inventory.container.Items[0].UpdateSlot(0, 8, _database);
            _inventory.AddItem(Wood, 5);
            
            Assert.AreEqual(10, _inventory.container.Items[0].amount);
            Assert.AreEqual(0, _inventory.container.Items[1].ID);
            Assert.AreEqual(3, _inventory.container.Items[1].amount);
        }

        [Test]
        public void AddItem_FillsAllMatchingPartialStacksBeforeNewSlots()
        {
            _inventory.container.Items[0].UpdateSlot(0, 8, _database); // space 2
            _inventory.container.Items[1].UpdateSlot(0, 7, _database); // space 3
            
            _inventory.AddItem(Wood, 5); // 2-> slot0, 3 -> slot1, nothing left over
            
            Assert.AreEqual(10, _inventory.container.Items[0].amount);
            Assert.AreEqual(10, _inventory.container.Items[1].amount);
            Assert.AreEqual(-1, _inventory.container.Items[2].ID); // no new slot needed
        }

        [Test]
        public void AddItem_WhenInventoryFull_SilentlyDropsExcess()
        {
            for (int i = 0; i < SlotCount; i++)
            {
                _inventory.container.Items[i].UpdateSlot(1, 5, _database);
            }
            
            Assert.DoesNotThrow(() => _inventory.AddItem(Stone, 1));

            for (int i = 0; i < SlotCount; i++)
                Assert.AreEqual(5, _inventory.container.Items[i].amount);
        }

        [Test]
        public void AddItem_ExceedingRemainingCapacity_FillsWhatFitsAndDropsRest()
        {
            // Only sot 3 is free; requesting 25 with MAxStack 10 means
            // 10 gets placed and 15 is silently last (current behavior).
            _inventory.container.Items[0].UpdateSlot(0, 10, _database);
            _inventory.container.Items[1].UpdateSlot(0, 10, _database);
            _inventory.container.Items[2].UpdateSlot(0, 10, _database);
            
            _inventory.AddItem(Wood, 25);
            
            Assert.AreEqual(0, _inventory.container.Items[3].ID);
            Assert.AreEqual(10, _inventory.container.Items[3].amount);
        }
        
        //-----------------------RemoveItem-----------------------

        [Test]
        public void RemoveItem_ClearsIdAmountAndCachedItem()
        {
            var slot = _inventory.container.Items[0];
            slot.UpdateSlot(0, 5, _database);
            
            _inventory.RemoveItem(slot);
            
            Assert.AreEqual(-1, slot.ID);
            Assert.AreEqual(0, slot.amount);
            Assert.IsNull(slot.item);
        }
        
        //------------------------MoveItem------------------------

        [Test]
        public void MoveItem_SameSlotReference_DoesNothing()
        {
            var slot = _inventory.container.Items[0];
            slot.UpdateSlot(0, 5, _database);
            
            _inventory.MoveItem(slot, slot);
            
            Assert.AreEqual(0, slot.ID);
            Assert.AreEqual(5, slot.amount);
        }

        [Test]
        public void MoveItem_DifferentItems_SwapsContents()
        {
            var SlotA = _inventory.container.Items[0];
            var SlotB = _inventory.container.Items[1];
            SlotA.UpdateSlot(0, 3, _database);
            SlotB.UpdateSlot(1, 2, _database);
            
            _inventory.MoveItem(SlotA, SlotB);
            
            Assert.AreEqual(1, SlotA.ID);
            Assert.AreEqual(2, SlotA.amount);
            Assert.AreEqual(0, SlotB.ID);
            Assert.AreEqual(3, SlotB.amount);
        }

        [Test]
        public void MoveItem_SameItem_MergesFullyWhenUnderMaxStack()
        {
            var SlotA = _inventory.container.Items[0];
            var SlotB = _inventory.container.Items[1];
            SlotA.UpdateSlot(0, 4, _database);
            SlotB.UpdateSlot(0, 3, _database);
            
            _inventory.MoveItem(SlotA, SlotB);
            
            Assert.AreEqual(7, SlotA.amount);
            Assert.AreEqual(-1, SlotB.ID);
            Assert.AreEqual(0, SlotB.amount);
        }

        [Test]
        public void MoveItems_SameItem_PartialMergeWhenExceedingMaxStack()
        {
            var SlotA = _inventory.container.Items[0];
            var SlotB = _inventory.container.Items[1];
            SlotA.UpdateSlot(0, 8, _database);
            SlotB.UpdateSlot(0, 5, _database);
            
            _inventory.MoveItem(SlotA, SlotB);

            Assert.AreEqual(10, SlotA.amount); // filled to max
            Assert.AreEqual(0, SlotB.ID);      // still Wood, not cleared
            Assert.AreEqual(3, SlotB.amount);  // 5 - 2 moved over
        }
        
        // -----------------ClearSlots -----------------

        [Test]
        public void ClearSlots_ResetsEveryContainerSlot()
        {
            _inventory.container.Items[0].UpdateSlot(0, 5, _database);
            _inventory.container.Items[2].UpdateSlot(1, 2, _database);

            _inventory.ClearSlots();

            foreach (var slots in _inventory.container.Items)
            {
                Assert.AreEqual(-1, slots.ID);
                Assert.AreEqual(0, slots.amount);
            }
        }
    }

    public class InventorySlotTests
    {
        private ItemDatabaseObject _database;

        [SetUp]
        public void SetUp()
        {
            var item = ScriptableObject.CreateInstance<TestItemsObject>();
            item.Id = 0;
            item.Name = "Wood";
            item.MaxStack = 10;
            
            _database = ScriptableObject.CreateInstance<ItemDatabaseObject>(); 
            _database.Items = new ItemsObject[] { item };
            _database.OnAfterDeserialize();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var item in _database.Items)
                Object.DestroyImmediate(item);
            Object.DestroyImmediate(_database);
        }

        [Test]
        public void CanPlaceInSlot_EmptySlot_ReturnsTrue()
        {
            var slot = new InventorySlot();
            Assert.IsTrue(slot.CanPlaceInSlot(null));
        }
        
        [Test]
        public void CanPlaceInSlot_Occupied_ReturnsFalse()
        {
            var slot = new InventorySlot(0, 5);
            Assert.IsFalse(slot.CanPlaceInSlot(null));
        }

        [Test]
        public void UpdateSlot_SetsIdAndCachedItem()
        {
            var slot = new InventorySlot();
            
            slot.UpdateSlot(0, 4, _database);
            
            Assert.AreEqual(0, slot.ID);
            Assert.AreEqual(4, slot.amount);
            Assert.IsNotNull(slot.item);
            Assert.AreEqual("Wood", slot.item.Name);
        }

        [Test]
        public void UpdateSlot_WithNegative_clearsCachedItem()
        {
            var slot = new InventorySlot();
            slot.UpdateSlot(0, 4, _database);

            slot.UpdateSlot(-1, 0, _database);
            
            Assert.IsNull(slot.item);
        }
        
        [Test]
        public void UpdateSlot_RaisesOnChangedEvent()
        {
            var slot = new InventorySlot();
            bool raised = false;
            slot.OnChanged += () => raised = true;
            
            slot.UpdateSlot(0, 1, _database);
            
            Assert.IsTrue(raised);
        }
        
        [Test]
        public void AddAmount_IncreasesAmountBySpecifiedValue()
        {
            var slot = new InventorySlot(0, 5);
            
            slot.AddAmount(3);
            
            Assert.AreEqual(8, slot.amount);
        }

    }
}