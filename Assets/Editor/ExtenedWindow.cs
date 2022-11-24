using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtenedWindow : EditorWindow
{
    protected SerializedObject SObject;
    protected SerializedProperty SProp;

    protected void DrawProps(SerializedProperty prop, bool DrawChildren)
    {
        string LastProp = string.Empty;
        foreach (SerializedProperty p in prop)
        {
            if(p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProps(p, DrawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(LastProp) && p.propertyPath.Contains(LastProp)) { continue; }
                LastProp = p.propertyPath;
                EditorGUILayout.PropertyField(p, DrawChildren);
            }

        }
    }
}
