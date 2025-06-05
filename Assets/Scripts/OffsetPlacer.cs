#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class OffsetPlacer : EditorWindow
{
    private Vector3 offset = Vector3.zero;

    [MenuItem("Tools/Offset Placer")]
    public static void ShowWindow()
    {
        GetWindow<OffsetPlacer>("Offset Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Offset Placer", EditorStyles.boldLabel);
        offset = EditorGUILayout.Vector3Field("Offset per Object:", offset);

        if (GUILayout.Button("Apply Offset"))
        {
            ApplyOffset();
        }
    }

    private void ApplyOffset()
    {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length < 2)
        {
            Debug.LogWarning("Select at least two objects to apply offset.");
            return;
        }

        // Sort by hierarchy order
        System.Array.Sort(selectedObjects, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        Vector3 startPosition = selectedObjects[0].transform.position;
        for (int i = 1; i < selectedObjects.Length; i++)
        {
            selectedObjects[i].transform.position = startPosition + (offset * i);
        }
    }
}
#endif