using System.Collections.Generic;
using Inventory.Items;
using UnityEngine;

namespace Inventory.Database
{
    [CreateAssetMenu(fileName =  "New Item Database", menuName = "Inventory System/Database")]
    public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemsObject[] Items;
        public Dictionary<int, ItemsObject> GetItem = new Dictionary<int, ItemsObject>();


        public void OnAfterDeserialize()
        {
            GetItem = new Dictionary<int, ItemsObject>();

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Id = i;
                GetItem.Add(i, Items[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            GetItem = new Dictionary<int, ItemsObject>();

        }
    }
}