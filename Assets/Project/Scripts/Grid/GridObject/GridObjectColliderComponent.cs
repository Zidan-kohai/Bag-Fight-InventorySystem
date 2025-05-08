using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Grid.GridObject
{
    public class GridObjectColliderComponent : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private List<GridObjectColliderComponent> gridsOtherCollider = new();
        [field: SerializeField] public InventoryItemView InventoryItemView { get; private set; }

        public void AddOtherCollider(GridObjectColliderComponent collider)
        {
            gridsOtherCollider.Add(collider);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out InventoryItemView inventoryItemView))
            {
                if (inventoryItemView.OnItemEnterSlot(this))
                {
                    Debug.Log(name + " " +  inventoryItemView.name + " OnEnter");
                    OnEnterSlot(inventoryItemView);
                }
            }
        }

        public void OnEnterSlot(InventoryItemView inventoryItemView)
        {
            Debug.Log(name + " " + inventoryItemView.name + " OnEnterSlot");

            if (inventoryItemView.GridObjectCollidersOnSlot.Any(x => gridsOtherCollider.Contains(x)))
            {
                Debug.Log("the inventory has other collider of this object");
                return;
            }

            if (this.InventoryItemView != null)
            {
                this.InventoryItemView.OnItemExitSlot(this);
            }

            this.InventoryItemView = inventoryItemView;
            Debug.Log("InventoryItemView " + InventoryItemView.name);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out InventoryItemView inventoryItemView))
            {
                Debug.Log(name + " " + inventoryItemView.name + " OnExit");
                if (inventoryItemView.OnItemExitSlot(this))
                {
                    this.InventoryItemView = null;
                }
            }
        }
    }
}