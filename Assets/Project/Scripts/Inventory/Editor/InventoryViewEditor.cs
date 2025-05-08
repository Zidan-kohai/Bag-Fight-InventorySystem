using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryView))]
public class InventoryViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Regenerate Inventary view"))
        {
            InventoryView inventoryView = (InventoryView)target;

            MethodInfo method = typeof(InventoryView).GetMethod("GenerateInventoryView", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(inventoryView, null);
            }
            else
            {
                Debug.LogWarning("Метод не найден");
            }
        }
        GUILayout.EndHorizontal();
    }
}
