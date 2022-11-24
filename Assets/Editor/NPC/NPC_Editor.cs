using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandeler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        TypeOfNPC npc = EditorUtility.InstanceIDToObject(instanceId) as TypeOfNPC;
        if(npc != null)
        {
            NPC_Window.OpenWindow(npc);
            return true;
        }
        return false;
    }
}

public class NPC_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open NPC"))
        {
            NPC_Window.OpenWindow((TypeOfNPC)target);
        }
    }
}
