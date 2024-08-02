using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Pool
{
    public class MonoBehaviourPool<T> where T : Component
    {
        public ReadOnlyCollection<T> UsedItems { get; private set; }

        private readonly List<T> _notUsedItems = new();
        private readonly List<T> _usedItems = new();

        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Func<T, Transform, T> _factoryMethod;
        private Func<T> _createMethod;

        public MonoBehaviourPool(Transform parent,
            Func<T> createMethod = null,
            T prefab = null,
            int defaultCount = 4,
            Func<T, Transform, T> factoryMethod = null )
        {
            _createMethod = createMethod;
            _factoryMethod = factoryMethod;
            _parent = parent;
            _prefab = prefab;
            if (_prefab == null && _createMethod != null)
            {
                _prefab = _createMethod.Invoke();
            }

            for (int i = 0; i < defaultCount; i++)
            {
                AddNewItemInPool();
            }

            UsedItems = new ReadOnlyCollection<T>(_usedItems);
        }
        

        public T Get()
        {
            if (_notUsedItems.Count == 0)
            {
                AddNewItemInPool();
            }

            var lastIndex = _notUsedItems.Count - 1;
            var itemFromPool = _notUsedItems[lastIndex];
            _notUsedItems.RemoveAt(lastIndex);
            _usedItems.Add(itemFromPool);
            itemFromPool.gameObject.SetActive(true);

            return itemFromPool;
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < _usedItems.Count; i++)
            {
                _usedItems[i].gameObject.SetActive(false);
            }

            _notUsedItems.AddRange(_usedItems);
            _usedItems.Clear();

            SortBySiblingIndexUnused();
        }

        private void SortBySiblingIndexUnused()
        {
            _notUsedItems.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));
        }

        public void Release(T item)
        {
            item.gameObject.SetActive(false);

            _usedItems.Remove(item);
            _notUsedItems.Add(item);
        }

        public void Release(List<T> items)
        {
            foreach (var item in items)
            {
                Release(item);
            }
        }

        private void AddNewItemInPool()
        {
            var newItem = _factoryMethod != null
                ? _factoryMethod(_prefab, _parent)
                : Object.Instantiate(_prefab, _parent, false);

            newItem.gameObject.SetActive(false);

            var returnToPoolOnDisable = newItem.GetComponent<ReturnToPoolOnDisable>();
            if (returnToPoolOnDisable != null)
            {
                returnToPoolOnDisable.Initialize(gameObject => Release(newItem));
            }

            _notUsedItems.Add(newItem);
        }
    }
}