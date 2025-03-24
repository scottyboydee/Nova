using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

public class AutoAssignWindow : EditorWindow
{
    private class FieldEntry
    {
        public GameObject gameObject;
        public MonoBehaviour component;
        public FieldInfo fieldInfo;
        public Component match;
        public bool selected = true;
    }

    private Vector2 scroll;
    private List<FieldEntry> entries = new List<FieldEntry>();
    private Dictionary<GameObject, bool> goFoldouts = new Dictionary<GameObject, bool>();
    private Dictionary<MonoBehaviour, bool> compFoldouts = new Dictionary<MonoBehaviour, bool>();

    [MenuItem("Tools/Auto-Assign Fields with Selection")]
    private static void ShowWindow()
    {
        var window = GetWindow<AutoAssignWindow>(true, "Auto-Assign Fields");
        window.minSize = new Vector2(500, 400);
        window.RefreshData();
    }

    private void RefreshData()
    {
        entries.Clear();
        goFoldouts.Clear();
        compFoldouts.Clear();

        foreach (GameObject go in Selection.gameObjects)
        {
            var components = go.GetComponents<MonoBehaviour>();

            foreach (var comp in components)
            {
                if (comp == null) continue; // Skip missing scripts

                var type = comp.GetType();
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                foreach (var field in fields)
                {
                    if (!field.IsDefined(typeof(SerializeField), true)) continue;
                    if (field.FieldType == typeof(string)) continue;
                    if (!typeof(Component).IsAssignableFrom(field.FieldType)) continue; // Ensure field type is a Component

                    var so = new SerializedObject(comp);
                    var prop = so.FindProperty(field.Name);
                    if (prop == null || prop.propertyType != SerializedPropertyType.ObjectReference || prop.objectReferenceValue != null) continue;

                    Component match = go.GetComponent(field.FieldType);
                    if (match != null)
                    {
                        entries.Add(new FieldEntry
                        {
                            gameObject = go,
                            component = comp,
                            fieldInfo = field,
                            match = match
                        });
                        if (!goFoldouts.ContainsKey(go)) goFoldouts[go] = true;
                        if (!compFoldouts.ContainsKey(comp)) compFoldouts[comp] = true;
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Refresh")) RefreshData();

        scroll = EditorGUILayout.BeginScrollView(scroll);

        foreach (var goGroup in GroupBy(entries, e => e.gameObject))
        {
            goFoldouts[goGroup.Key] = EditorGUILayout.Foldout(goFoldouts[goGroup.Key], goGroup.Key.name, true);
            if (!goFoldouts[goGroup.Key]) continue;
            EditorGUI.indentLevel++;

            foreach (var compGroup in GroupBy(goGroup.Value, e => e.component))
            {
                compFoldouts[compGroup.Key] = EditorGUILayout.Foldout(compFoldouts[compGroup.Key], compGroup.Key.GetType().Name, true);
                if (!compFoldouts[compGroup.Key]) continue;
                EditorGUI.indentLevel++;

                foreach (var entry in compGroup.Value)
                {
                    entry.selected = EditorGUILayout.ToggleLeft($"{entry.fieldInfo.Name} ? {entry.match.GetType().Name}", entry.selected);
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Assign Checked Fields"))
        {
            AssignSelected();
            RefreshData();
        }
    }

    private void AssignSelected()
    {
        foreach (var entry in entries)
        {
            if (!entry.selected) continue;

            var so = new SerializedObject(entry.component);
            var prop = so.FindProperty(entry.fieldInfo.Name);
            if (prop != null && prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                Undo.RecordObject(entry.component, "Auto-Assign Field");
                prop.objectReferenceValue = entry.match;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(entry.component);
                Debug.Log($"Assigned {entry.fieldInfo.Name} on {entry.component.GetType().Name} in {entry.gameObject.name}");
            }
        }
    }

    private static Dictionary<TKey, List<TElement>> GroupBy<TElement, TKey>(IEnumerable<TElement> source, Func<TElement, TKey> keySelector)
    {
        var dict = new Dictionary<TKey, List<TElement>>();
        foreach (var element in source)
        {
            var key = keySelector(element);
            if (!dict.ContainsKey(key)) dict[key] = new List<TElement>();
            dict[key].Add(element);
        }
        return dict;
    }
}
