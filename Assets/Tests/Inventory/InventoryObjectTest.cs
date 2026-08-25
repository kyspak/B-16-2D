// using Inventory;
// using Inventory.Database;
// using Inventory.Items;
// using NUnit.Framework;
//
// namespace Tests.Inventory
// {
//     public class InventoryObjectTest
//     {
//         private ItemDatabaseObject _database;
//         private InventoryObject _inventory;
//
//         [SetUp]
//         public void Setup()
//         {
//             _database = TestHelpers.CreateTestDatabase(10, 1);
//             _inventory = TestHelpers.CreateTestInventory(_database, 3);
//         }
//
//         [TearDown]
//         public void TearDown()
//         {
//             TestHelpers.Destroy(_inventory);
//             TestHelpers.Destroy(_database);
//         }
//
//         private Item GetItem(int id)
//         {
//             return _database.GetItem[id].CreateItem();
//             
//         }
//
//         [Test]
//         public void AddItem_PutsItemIntoEmptySlot()
//         {
//             var item = GetItem(0);
//
//             _inventory.AddItem(item, 3);
//
//             Assert.AreEqual(0, _inventory.container.Items[0].ID);
//             Assert.AreEqual(3, _inventory.container.Items[0].amount);
//         }
//
//         [Test]
//         public void AddItem_AddsToExistingStack()
//         {
//             _inventory.containersItems[0].UpdateSlot(0, 4, _database);
//             var item = GetItem(0);
//             
//             _inventory.AddItem(item, 3);
//             
//             Assert.AreEqual(7, _inventory.containersItems.Items[0].amount);
//             
//         }
//
//         [Test]
//         public void AddItem_CreateNewStack_WhenCurrentStackIsFull()
//         {
//             _inventory.containers.Items[0].UpdateSlot(0, 9, _database);
//             var item = GetItem(0);
//             
//             _inventory.AddItem(item, 5);
//             
//             Assert.AreEqual(10, _inventory.container.Items[0].amount);
//             
//             Assert.AreEqual(0, _inventory.container.Items[1].ID);
//             Assert.AreEqual(4, _inventory.container.Items[0].amount);
//         }
//
//         [Test]
//         public void AddItem_SplitsItemsBetweenSlots()
//         {
//             var item = GetItem(0);
//             _inventory.AddItem(item, 25);
//
//             Assert.AreEqual(10, _inventory.container.Items[0].amount);
//             Assert.AreEqual(5, _inventory.container.Items[1].amount);
//             Assert.AreEqual(5, _inventory.container.Items[2].amount);
//         }
//
//         [Test]
//         public void AddItem_DoesNothing_WhenInventoryIsFull()
//         {
//             var item = GetItem(0);
//
//             for (int i = 0; i < 3; i++)
//             {
//                 _inventory.container.Items[i].UpdateSlot(1,1, _database);
//                 
//             }
//             
//             Assert.DoesNotThrow(() => _inventory.AddItem(item, 5));
//         }
//
//         [Test]
//         public void MoveItem_SwapDifferentItems()
//         {
//             _inventory.container.Items[0].UpdateSlot(0, 4, _database);
//             
//             _inventory.container.Items[1].UpdateSlot(1, 1, _database);
//
//             _inventory.MoveItem(_inventory.container.Items[1], _inventory.container.Items[0]);
//             
//             Assert.AreEqual(1, _inventory.container.Items[0].ID);
//             Assert.AreEqual(1, _inventory.container.Items[0].amount);
//
//             Assert.AreEqual(1, _inventory.container.Items[1].ID);
//             Assert.AreEqual(1, _inventory.container.Items[0].amount);
//
//
//         }
//     }
// }