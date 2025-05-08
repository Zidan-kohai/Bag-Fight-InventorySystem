using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Grid.GridObject
{
    public class GridObjectView : MonoBehaviour
    {
        [SerializeField] private GridObject gridObject;
        [SerializeField] private GridObjectColliderComponent gridObjectColliderComponentPrefab;
        [SerializeField] private RectTransform contentLayout;

        private void Start()
        {
            GenerateInventoryView();
        }

        private void GenerateInventoryView()
        {
            gridObject.DestoryColliders();

            var contentLayoutWidth = contentLayout.rect.size.x;
            var contentLayoutHeight = contentLayout.rect.size.y;

            var biggerWidth = gridObject.GridConfig.Grid.OrderBy(x => x.Count).First().Count;

            var ySpace = contentLayoutHeight / gridObject.GridConfig.Grid.Count;
            var xSpace = contentLayoutWidth / biggerWidth;

            Vector2 pos = new Vector3(0, (contentLayoutHeight / 2) - (ySpace / 2));

            for (int i = 0; i < gridObject.GridConfig.Grid.Count; i++)
            {
                pos = new Vector2(-(contentLayoutWidth / 2) + (xSpace / 2), pos.y);

                for (int j = 0; j < gridObject.GridConfig.Grid[i].Count; j++)
                {
                    if (gridObject.GridConfig.Grid[i][j] == 1)
                        InstantiateItem(pos, i, j);

                    pos += new Vector2(xSpace, 0);
                }

                pos += new Vector2(0, -ySpace);
            }

            gridObject.CreateConnectionBetweenCollider();
        }

        private void InstantiateItem(Vector2 pos, int height, int width)
        {
            var instanceItem = Instantiate(gridObjectColliderComponentPrefab, contentLayout); 
            instanceItem.name = $"GridObjectSlot_{height}_{width}";

            var instanceItemRect = instanceItem.transform as RectTransform;

            instanceItemRect.localScale = Vector2.one;

            instanceItemRect.anchoredPosition = pos;

            gridObject.AddCollider(instanceItem);
        }
    }
}