using Scripts.Grid;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(GridConfig))]
public class GridConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        GridConfig gridConfig = target as GridConfig;

        float totalWidth = EditorGUIUtility.currentViewWidth;
        int rowCount = gridConfig.Grid.Count;
        float padding = 5f;


        for (int i = 0; i < gridConfig.Grid.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            for(int j = 0; j < gridConfig.Grid[i].Count; j++)
            {
                int columnCount = gridConfig.Grid[i].Count;
                float buttonSize = (totalWidth - (columnCount + 1) * padding) / columnCount;

                Color originalColor = GUI.color;
                
                GUI.color = gridConfig.Grid[i][j] == 1 ? Color.green : Color.red;

                if(GUILayout.Button($"{i}:{j}", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    OnClickButton(gridConfig.Grid, i, j);
                }

                GUI.color = originalColor;
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void OnClickButton(List<List<int>> grid, int i, int j)
    {
        grid[i][j] = grid[i][j] == 1 ? 0 : 1;
    }
}
