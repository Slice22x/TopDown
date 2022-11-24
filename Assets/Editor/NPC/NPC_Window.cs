using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NPC_Window : ExtenedWindow
{
    public static void OpenWindow(TypeOfNPC NPC)
    {
        NPC_Window npc_Window = GetWindow<NPC_Window>("NPC Editor");
        npc_Window.SObject = new SerializedObject(NPC);
    }

    private void OnGUI()
    {
        DrawP("Personalities", false);
        DrawP("Role", false);
    }

    void DrawP(string prop, bool child)
    {
        SProp = SObject.FindProperty(prop);
        DrawProps(SProp, child);
    }
}
