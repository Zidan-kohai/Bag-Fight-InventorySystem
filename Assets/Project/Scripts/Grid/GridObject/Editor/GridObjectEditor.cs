using UnityEditor;

namespace Scripts.Grid.GridObject.Editor
{
    [CustomEditor(typeof(GridObject))]
    public class GridObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridObject gridObject = (GridObject)target;

            if (gridObject.GridConfig != null)
            {
                UnityEditor.Editor editor = CreateEditor(gridObject.GridConfig);

                editor.OnInspectorGUI();
            }
        }
    }
}
