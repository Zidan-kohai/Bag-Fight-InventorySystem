using Scripts.Grid.GridObject;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Image image;
    [SerializeField] public GridObjectColliderComponent GridObjectColliderIntoSlot { get; private set; }
    [field: SerializeField] public List<GridObjectColliderComponent> GridObjectCollidersOnSlot { get; private set; } = new();
    public RectTransform GridObjectParent { get; private set; }

    private void Start()
    {
        boxCollider2D.size = (transform as RectTransform).sizeDelta;
    }

    public void Init(RectTransform gridObjectParent)
    {
        GridObjectParent = gridObjectParent;
    }

    public bool OnItemEnterSlot(GridObjectColliderComponent gridObjectColliderComponent)
    {
        bool result = false;

        if (!GridObjectCollidersOnSlot.Contains(gridObjectColliderComponent))
        {
            GridObjectCollidersOnSlot.Add(gridObjectColliderComponent);
            
            if(GridObjectColliderIntoSlot == null) 
                result = true;
        }

        if (GridObjectColliderIntoSlot == null)
        {
            image.color = Color.green;

            
        }
        else
            image.color = Color.red;


        return result;
    }

    public bool OnItemExitSlot(GridObjectColliderComponent gridObjectColliderComponent)
    {
        bool result = false;

        if (GridObjectCollidersOnSlot.Contains(gridObjectColliderComponent))
        {
            result = true;

            GridObjectCollidersOnSlot.Remove(gridObjectColliderComponent);

            if (GridObjectCollidersOnSlot.Count > 0)
                GridObjectCollidersOnSlot[0].OnEnterSlot(this);
        }

        if(GridObjectCollidersOnSlot.Count > 0 && GridObjectColliderIntoSlot != null)
        {
            image.color = Color.red;
        }
        else if(GridObjectColliderIntoSlot != null)
        {
            image.color = Color.yellow;
        }
        else if (GridObjectCollidersOnSlot.Count == 0)
        {
            image.color = Color.white;
        }
        return result;
    }

    public void OnItemDropIntoSlot(GridObjectColliderComponent gridObjectColliderComponent)
    {
        GridObjectColliderIntoSlot = gridObjectColliderComponent;

        if (GridObjectCollidersOnSlot.Contains(gridObjectColliderComponent))
            GridObjectCollidersOnSlot.Remove(gridObjectColliderComponent);

        image.color = Color.yellow;
    }

    public void OnItemTakeFromSlot(GridObjectColliderComponent gridObjectColliderComponent)
    {
        if (GridObjectColliderIntoSlot == gridObjectColliderComponent)
        {
            if (!GridObjectCollidersOnSlot.Contains(gridObjectColliderComponent))
                GridObjectCollidersOnSlot.Add(gridObjectColliderComponent);

            GridObjectColliderIntoSlot = null;
        }
    }
}