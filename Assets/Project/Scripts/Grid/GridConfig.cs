using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Grid
{
    [CreateAssetMenu(fileName = "Grid pattern", menuName = "SO/Grid")]
    public class GridConfig : ScriptableObject, ISerializationCallbackReceiver
    {
        [Min(1)]
        [SerializeField] private int Height;
        [Min(1)]
        [SerializeField] private int Width;

        [field: SerializeField] public List<List<int>> Grid { get; private set; }

        [SerializeField, HideInInspector] private string GridData;

        private void OnValidate()
        {
            if (Grid == null)
                InitializeGrid();

            if (Grid.Count < Height)
                while (Grid.Count != Height)
                    Grid.Add(new List<int>() { 1 });

            else if (Grid.Count > Height)
                while (Grid.Count != Height)
                    Grid.Remove(Grid[Grid.Count - 1]);

            for (int i = 0; i < Grid.Count; i++)
            {
                if (Grid[i].Count < Width)
                {
                    while (Grid[i].Count != Width)
                        Grid[i].Add(1);
                }
            }

            for (int i = 0; i < Grid.Count; i++)
            {
                if (Grid[i].Count > Width)
                {
                    while (Grid[i].Count != Width)
                        Grid[i].RemoveAt(Grid[i].Count - 1);
                }
            }
        }

        private void InitializeGrid()
        {
            Grid = new List<List<int>>(Height);
            for (int i = 0; i < Height; i++)
            {
                List<int> column = new();
                for (int j = 0; j < Width; j++)
                {
                    column.Add(1);
                }
                Grid.Add(column);
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            GridData = JsonConvert.SerializeObject(Grid);

            if (Grid == null)
                InitializeGrid();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Grid = JsonConvert.DeserializeObject<List<List<int>>>(GridData);
        }
    }
}