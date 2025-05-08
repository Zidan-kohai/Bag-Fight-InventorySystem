using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    private enum ItemSizeSetting
    {
        OriginalSize,
        ScaleToFit,
        ScaleWithAspect
    }

    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private InventoryItemView inventoryItemViewPrefab;
    [SerializeField] private RectTransform contentLayout;
    [SerializeField] private RectTransform gridObjectLayout;

    [Header("Settings")]
    [SerializeField] private float xSpaceBetweenItem;
    [SerializeField] private float ySpaceBetweenItem;
    [SerializeField] private ItemSizeSetting itemSizeSetting;

    private List<InventoryItemView> items = new();

    private void Start()
    {
        GenerateInventoryView();
    }

    private void GenerateInventoryView()
    {
        DestoryLastItems();

        var contentLayoutWidth = contentLayout.rect.size.x;
        var contentLayoutHeight = contentLayout.rect.size.y;

        int itemSizeWidth = (int)(inventoryItemViewPrefab.transform as RectTransform).sizeDelta.x;
        int itemSizeHeight = (int)(inventoryItemViewPrefab.transform as RectTransform).sizeDelta.y;

        var biggerWidth = inventoryData.GridConfig.Grid.OrderBy(x => x.Count).First().Count;

        if (itemSizeSetting == ItemSizeSetting.ScaleToFit)
        {
            itemSizeHeight = (int)((contentLayoutHeight - (inventoryData.GridConfig.Grid.Count * ySpaceBetweenItem)) / inventoryData.GridConfig.Grid.Count );

            itemSizeWidth = (int)((contentLayoutWidth - (biggerWidth * xSpaceBetweenItem)) / biggerWidth );
        }

        int requiredHeight = (int)(itemSizeHeight * inventoryData.GridConfig.Grid.Count + ySpaceBetweenItem * inventoryData.GridConfig.Grid.Count);
        var requiredWidth = (int)(itemSizeWidth * biggerWidth + xSpaceBetweenItem * biggerWidth);

        if (itemSizeSetting == ItemSizeSetting.ScaleWithAspect)
        {
            float factor = (float)itemSizeHeight / itemSizeWidth;

            if (contentLayoutWidth < requiredWidth)
            {
                itemSizeWidth = (int)((contentLayoutWidth - (biggerWidth * xSpaceBetweenItem)) / biggerWidth);
                itemSizeHeight = (int)(itemSizeWidth * factor);
            }
            if(contentLayoutHeight < requiredHeight)
            {
                itemSizeHeight = (int)((contentLayoutHeight - (inventoryData.GridConfig.Grid.Count * ySpaceBetweenItem)) / inventoryData.GridConfig.Grid.Count);
                itemSizeWidth = (int)(itemSizeHeight / factor);
            }

            requiredHeight = (int)(itemSizeHeight * inventoryData.GridConfig.Grid.Count + ySpaceBetweenItem * inventoryData.GridConfig.Grid.Count);
            requiredWidth = (int)(itemSizeWidth * biggerWidth + xSpaceBetweenItem * biggerWidth);
        }

        if(contentLayoutWidth < requiredWidth || contentLayoutHeight < requiredHeight)
        {
            Debug.LogWarning("The size of content dont enough");
            return;
        }

        Vector2 pos = new Vector3(0, requiredHeight / 2 + ySpaceBetweenItem / 2);

        for (int i = 0; i < inventoryData.GridConfig.Grid.Count; i++)
        {
            pos = new Vector2(-(requiredWidth / 2) - xSpaceBetweenItem / 2, pos.y - ySpaceBetweenItem - (itemSizeHeight / 2));

            for (int j = 0; j < inventoryData.GridConfig.Grid[i].Count; j++)
            {
                pos += new Vector2(xSpaceBetweenItem + (itemSizeWidth / 2), 0);

                if (inventoryData.GridConfig.Grid[i][j] == 1)
                    InstantiateItem(pos, itemSizeWidth, itemSizeHeight, i, j);

                pos += new Vector2(itemSizeWidth / 2, 0);
            }

            pos += new Vector2(0, -(itemSizeHeight / 2));
        }

    }

    private void DestoryLastItems()
    {
        while(items.Count != 0)
        {
            var item = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);

            if (item != null)
                DestroyImmediate(item.gameObject);
        }
    }

    private void InstantiateItem(Vector2 pos, int itemSizeWidth, int itemSizeHeight, int height, int width)
    {
        var instanceItem = Instantiate(inventoryItemViewPrefab, contentLayout);
        instanceItem.name = $"InventorySlot_{height}_{width}";

        var instanceItemRect = instanceItem.transform as RectTransform;

        instanceItemRect.localScale = Vector2.one;

        instanceItemRect.anchoredPosition = pos;
        instanceItemRect.sizeDelta = new Vector2(itemSizeWidth, itemSizeHeight);

        instanceItem.Init(gridObjectLayout);

        items.Add(instanceItem);
    }
}
