using Scripts.Grid;
using System.Collections.Generic;
using UnityEngine;

public class InventoryData : MonoBehaviour
{
    [field: SerializeField] public GridConfig GridConfig { get; private set; }

    private List<List<int>> grid = new();

    public IEnumerable<IEnumerable<int>> Grid => grid;

    private void Start()
    {
        for(int i = 0; i < GridConfig.Grid.Count; i++)
        {
            grid.Add(new List<int>());

            for(int j =0; j < GridConfig.Grid[i].Count; j++)
            {
                grid[i].Add(GridConfig.Grid[i][j]);
            }
        }
    }

}
