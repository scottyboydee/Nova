using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

public class SafeMultiEditTool : EditorWindow
{
    private GameObject[] selectedObjects;
    private List<Type> availableComponentTypes = new();
    private string[] componentTypeNames = Array.Empty<string>();
    private int selectedComponentTypeIndex = 0;

    private List<Component> targetComponents = new();
    private string[] fieldNames = Array.Empty<string>();
    private int selectedFieldIndex = 0;
    private string newValue = "";

    [MenuItem("Tools/Safe Multi-Edit")]
    static void Init() => GetWindow<SafeMultiEditTool>("Safe Multi-Edit");

    void OnFocus() => RefreshSelection();
    void OnSelectionChange() => RefreshSelection();

    void RefreshSelection()
    {
        selectedObjects = Selection.gameObjects;
        availableComponentTypes.Clear();
        targetComponents.Clear();
        componentTypeNames = Array.Empty<string>();
        fieldNames = Array.Empty<string>();
        selectedComponentTypeIndex = 0;

        if (selectedObjects.Length == 0)
            return;

        // Find all MonoBehaviour types common to all selected GameObjects
        var commonTypes = new HashSet<Type>(GetCustomComponentTypes(selectedObjects[0]));

        foreach (var go in selectedObjects)
            commonTypes.IntersectWith(GetCustomComponentTypes(go));

        availableComponentTypes.AddRange(commonTypes);
        componentTypeNames = availableComponentTypes.ConvertAll(t => t.Name).ToArray();

        if (availableComponentTypes.Count > 0)
            UpdateTargetComponents();
    }

    static IEnumerable<Type> GetCustomComponentTypes(GameObject go)
    {
        foreach (var mb in go.GetComponents<MonoBehaviour>())
        {
            if (mb == null) continue; // skip broken/missing scripts

            var type = mb.GetType();
            // safer namespace check (some user types have null namespace)
            if (type.Namespace == null || !type.Namespace.StartsWith("UnityEngine"))
                yield return type;
        }
    }


    void UpdateTargetComponents()
    {
        if (availableComponentTypes.Count == 0)
            return;

        var type = availableComponentTypes[selectedComponentTypeIndex];
        targetComponents.Clear();

        foreach (var go in selectedObjects)
        {
            var comp = go.GetComponent(type);
            if (comp != null)
                targetComponents.Add(comp);
        }

        List<string> names = new();
        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (field.IsPublic || field.IsDefined(typeof(SerializeField), true))
                names.Add(field.Name);
        }

        fieldNames = names.ToArray();
        selectedFieldIndex = 0;
    }

    void OnGUI()
    {
        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorGUILayout.LabelField("Select GameObjects with shared custom components.");
            return;
        }

        if (availableComponentTypes.Count == 0)
        {
            EditorGUILayout.LabelField("No shared custom components found.");
            return;
        }

        EditorGUILayout.LabelField("Component Type:");
        int newIndex = EditorGUILayout.Popup(selectedComponentTypeIndex, componentTypeNames);
        if (newIndex != selectedComponentTypeIndex)
        {
            selectedComponentTypeIndex = newIndex;
            UpdateTargetComponents();
        }

        if (fieldNames.Length == 0)
        {
            EditorGUILayout.LabelField("No editable fields found.");
            return;
        }

        selectedFieldIndex = EditorGUILayout.Popup("Field:", selectedFieldIndex, fieldNames);
        newValue = EditorGUILayout.TextField("New Value:", newValue);

        if (GUILayout.Button("Apply to All"))
            ApplyValueToAll();
    }

    void ApplyValueToAll()
    {
        var type = availableComponentTypes[selectedComponentTypeIndex];
        var field = type.GetField(fieldNames[selectedFieldIndex], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (field == null)
        {
            Debug.LogError("Field not found.");
            return;
        }

        foreach (var comp in targetComponents)
        {
            Undo.RecordObject(comp, "Safe Multi-Edit");

            try
            {
                object parsed = field.FieldType switch
                {
                    var t when t == typeof(string) => newValue,
                    var t when t == typeof(int) => int.Parse(newValue),
                    var t when t == typeof(float) => float.Parse(newValue),
                    var t when t == typeof(bool) => bool.Parse(newValue),
                    _ => throw new NotSupportedException($"Type {field.FieldType} not supported")
                };

                field.SetValue(comp, parsed);
                EditorUtility.SetDirty(comp);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error applying value: {e.Message}");
            }
        }
    }
}
