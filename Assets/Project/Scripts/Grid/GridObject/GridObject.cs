using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Grid.GridObject
{
    public class GridObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField] public GridConfig GridConfig { get; private set; }
        private List<GridObjectColliderComponent> colliders = new();
        private RectTransform slotOnPool;

        public void Init(RectTransform slotPos)
        {
            slotOnPool = slotPos;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * 1.2f;

            if (colliders.TrueForAll(x => x.InventoryItemView != null))
            {
                OnTakeItemFromInventorySlot();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var rect = transform as RectTransform;
            rect.anchoredPosition += eventData.delta;


        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;

            if (colliders.TrueForAll(x => x.InventoryItemView != null))
            {
                DropIntoInventarySlot();
            }
            else
                ReturnToPool();

        }

        private void ReturnToPool()
        {
            transform.parent = slotOnPool;
            transform.position = slotOnPool.position;
        }

        private void OnTakeItemFromInventorySlot()
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].InventoryItemView.OnItemTakeFromSlot(colliders[i]);

                colliders[i].InventoryItemView.OnItemEnterSlot(colliders[i]);
            }
        }

        private void DropIntoInventarySlot()
        {
            float xCenter = 0;
            float yCenter = 0;

            for(int i = 0; i < colliders.Count; i++)
            {
                xCenter += colliders[i].InventoryItemView.transform.position.x;
                yCenter += colliders[i].InventoryItemView.transform.position.y;

                colliders[i].InventoryItemView.OnItemDropIntoSlot(colliders[i]);
            }

            xCenter /= colliders.Count;
            yCenter /= colliders.Count;

            transform.position = new Vector3(xCenter, yCenter, 0);
            transform.parent = colliders[0].InventoryItemView.GridObjectParent;
        }

        public void AddCollider(GridObjectColliderComponent instanceItem)
        {
            if(!colliders.Contains(instanceItem))
                colliders.Add(instanceItem);
        }

        public void DestoryColliders()
        {
            while (colliders.Count != 0)
            {
                var item = colliders[colliders.Count - 1];
                colliders.RemoveAt(colliders.Count - 1);

                if (item != null)
                    DestroyImmediate(item.gameObject);
            }
        }

        public void CreateConnectionBetweenCollider()
        {
            for(int i = 0; i < colliders.Count; i++)
            {
                for(int j = 0; j < colliders.Count; j++)
                {
                    if (i == j)
                        continue;

                    colliders[i].AddOtherCollider(colliders[j]);
                }    
            }
        }
    }
}


