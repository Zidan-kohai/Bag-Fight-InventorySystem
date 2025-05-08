using Scripts.Grid.GridObject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ItemPool
{
    public class ItemPool : MonoBehaviour
    {
        [Serializable]
        private struct ItemAndSlot
        {
            public RectTransform SlotPos;
            public GridObject GridObjectPrefab;
        }

        [SerializeField] private List<ItemAndSlot> pool;

        private void Start()
        {
            CreateGridObject();
        }

        private void CreateGridObject()
        {
            for(int i = 0; i < pool.Count; i++)
            {
                GridObject item = Instantiate(pool[i].GridObjectPrefab, pool[i].SlotPos);
                item.Init(pool[i].SlotPos);
            }
        }
    }
}