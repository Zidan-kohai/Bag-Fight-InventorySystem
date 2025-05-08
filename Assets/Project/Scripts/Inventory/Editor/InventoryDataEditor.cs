using UnityEditor;

[CustomEditor(typeof(InventoryData))]
public class InventoryDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InventoryData inventoryData = (InventoryData)target;

        if (inventoryData.GridConfig != null)
        {
            Editor editor = CreateEditor(inventoryData.GridConfig);

            editor.OnInspectorGUI();
        }
    }
}
