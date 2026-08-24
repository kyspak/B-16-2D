// using Inventory.Database;
// using Inventory.Items;
// using Tests.Inventory;
// using UnityEngine;
//
// namespace Inventory
// {
//     public static class TestHelpers
//     {
//         public static ItemDatabaseObject CreateTestDatabase(params int[] maxStack)
//         {
//             var database = ScriptableObject.CreateInstance<ItemDatabaseObject>();
//             
//             database.Items = new ItemsObject[maxStack.Length];
//             for (int i = 0; i < maxStack.Length; i++)
//             {
//                 var item = ScriptableObject.CreateInstance<TestItemObject>();
//
//                 item.Name = $"Test Item_{i}";
//                 item.MaxStack = maxStack[i];
//                 database.Items[i] = item;
//             }
//             
//             database.OnAfterDeserialize();
//             
//             return database;
//         }
//
//         public static InventoryObject CreateTestInventory(ItemDatabaseObject database, int size)
//         {
//             var inventory = ScriptableObject.CreateInstance<InventoryObject>();
//
//             inventory.database = database;
//             inventory.inventorySize = size;
//
//             inventory.container = new Inventory();
//             inventory.container.Initialize(size);
//
//             foreach (var slot in inventory.container.Items)
//                 slot.Bind(database);
//             
//             return inventory;
//
//         }
//
//         public static void DestroyDatabase(ItemDatabaseObject database)
//         {
//             if (!database)
//                 return;
//             if (database.Items != null)
//             {
//                 foreach (var item in database.Items)
//                 {
//                     if(item)
//                         Object.DestroyImmediate(item);
//                     Object.DestroyImmediate(database);
//                 }
//             }
//             
//         }
//
//         public static void Destroy(params Object[] objects)
//         {
//             foreach (var obj in objects)
//             {
//                 if(obj)
//                     Object.DestroyImmediate(obj);
//             }
//             
//         }
//     }
// }