// using Inventory;
// using Inventory.Container;
// using Inventory.Database;
// using JetBrains.Annotations;
// using NUnit.Framework;
//
//
// namespace Inventory.Tests
// {
//     public class InventorySlotTest
//     {
//         private ItemDatabaseObject _database;
//
//         [SetUp]
//         public void Setup()
//         {
//             _database = TestHelpers.CreateTestDatabase(10);
//         }
//
//         [TearDown]
//         public void TearDown()
//         {
//             TestHelpers.DestroyDatabase(_database);
//         }
//
//         [Test]
//         public void NewSlot_IsEmpty()
//         {
//             var slot = new InventorySlot();
//             
//             Assert.AreEqual(-1, slot.ID);
//             Assert.AreEqual(0, slot.amount);
//         }
//
//         [Test]
//         public void UpdateSlot_SetsIdAndAmount()
//         {
//             var slot = new InventorySlot();
//
//             slot.UpdateSlot(0, 5, _database);
//             
//             Assert.AreEqual(0, slot.ID);
//             Assert.AreEqual(5, slot.amount);
//         }
//
//         [Test]
//         public void UpdateSlot_InvokeOnChange()
//         {
//             var slot = new InventorySlot();
//             bool changed = false;
//
//             slot.OnChange += () => changed = true;
//
//             slot.UpdateSlot(0, 5, _database);
//             
//             Assert.IsTrue(changed);
//         }
//
//         [Test]
//         public void AddAmount_IncreaseAmount()
//         {
//             var slot = InventorySlot();
//             
//             slot.UpdateSlot(0, 5, _database);
//
//             slot.AddAmount(2);
//             
//             Assert.AreEqual(7, slot.amount);
//             
//             
//         }
//
//         [Test]
//         public void AddAmount_InvokeOnChange()
//         {
//             var slot = InventorySlot();
//             
//             slot.UpdateSlot(0, 5, _database);
//
//             int callCount = 0;
//             slot.OnChange += () => callCount++;
//
//             slot.Amount(2);
//             
//             Assert.AreEqual(7, callCount);
//         }
//
//         [Test]
//         public void TryConsume_RemoveOneItem()
//         {
//             var slot = new InventorySlot();
//             slot.UpdateSlot(0, 5, _database);
//
//             bool result = slot.TryConsume();
//
//             Assert.IsTrue(result);
//             Assert.AreEqual(4, slot.amount);
//         }
//
//         [Test]
//         public void TryConsume_ReturnFalse_WhenOnlyOneItemLeft()
//         {
//             var slot = new InventorySlot();
//             slot.UpdateSlot(0, 5, _database);
//
//             bool result = slot.TryConsume();
//             Assert.IsFalse(result);
//             Assert.AreEqual(1, slot.amount);
//         }
//
//         [Test]
//         public void TryConsume_ReturnFalse_WhenSlotIsNull()
//         {
//             InventorySlot slot = null;
//
//             bool result = slot.TryConsume();
//             Assert.IsFalse(result);
//         }
//     }
// }