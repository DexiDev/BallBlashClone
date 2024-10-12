using System;
using System.Collections.Generic;
using DexiDev.Game.Items;
using DexiDev.Game.Levels;
using Sirenix.Utilities;
using VContainer;

namespace DexiDev.Game.Inventory
{
    public class InventoryManager
    {
        [Inject] private ItemsManager _itemsManager;
        [Inject] private LevelManager _levelManager;
        
        private Dictionary<string, int> _items = new();
        
        public event Action<string, int> OnItemChanged;
        public event Action<string, int> OnItemAdded;
        public event Action<string, int> OnItemRemoved;

        public void Add(string itemID, int count)
        {
            var itemData = _itemsManager.GetData(itemID);

            if (itemData != null)
            {
                _items.TryAdd(itemID, 0);

                var resultCount = _items[itemID] + count;

                if (itemData.StackLimit != -1)
                {
                    if (resultCount > itemData.StackLimit)
                    {
                        resultCount = itemData.StackLimit;
                    }
                    else resultCount = Math.Clamp(resultCount, resultCount, itemData.StackLimit);
                }

                var oldCount = _items[itemID]; 
                
                _items[itemID] = resultCount;
                
                OnItemChanged?.Invoke(itemID, resultCount);
                OnItemAdded?.Invoke(itemID, resultCount - oldCount);
            }
        }

        public void Remove(string itemID, int count)
        {
            if (_items.ContainsKey(itemID))
            {
                var resultCount = _items[itemID] - count;
                resultCount = Math.Clamp(resultCount, 0, resultCount);
                    
                var oldCount = _items[itemID];
                    
                _items[itemID] = resultCount;
                    
                OnItemChanged?.Invoke(itemID, resultCount);
                OnItemRemoved?.Invoke(itemID, resultCount - oldCount);
            }
        }

        public bool ContainsItem(string targetItem, int count = 1)
        {
            return GetItemCount(targetItem) >= count;
        }

        public int GetItemCount(string targetItem)
        {
            return _items.GetValueOrDefault(targetItem, 0);
        }

        public int GetItemAllCount()
        {
            var items = GetItems();

            int count = 0;

            items?.ForEach(item => count += item.Value);

            return count;
        }

        public Dictionary<string, int> GetItems()
        {
            return _items;
        }
    }
}